using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Brainshare.Infrastructure.Databases;
using Brainshare.Infrastructure.Platform.Domain.Transitions;
using Brainshare.Infrastructure.Platform.Domain.Transitions.Interfaces;
using Brainshare.Infrastructure.Platform.Domain.Transitions.Mongo;
using Brainshare.Infrastructure.Platform.Output;
using Brainshare.Infrastructure.Platform.Replay;
using MongoDB.Driver;
using NLog;

namespace Brainshare.Infrastructure.Platform.Upgrade
{
    public class UpgradeManager
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ITransitionRepository _fromRepository;
        private readonly ITransitionRepository _toRepository;
        private readonly Brainshare.Infrastructure.Settings.Settings _settings;
        private readonly MongoEventsDatabase _events;
        private readonly IEnumerable<IUpgrader> _upgraders;
        public readonly IOutputWriter OutputWriter;

        private IInputReader InputReader
        {
            get { return OutputWriter as IInputReader; }
        }

        #region Internal staff of UpgradeManager

        public UpgradeManager(Brainshare.Infrastructure.Settings.Settings settings, MongoEventsDatabase events, IEnumerable<IUpgrader> upgraders, IOutputWriter outputWriter)
        {
            _settings = settings;
            _events = events;
            _upgraders = upgraders;
            OutputWriter = outputWriter;

            _fromRepository = new MongoTransitionRepository(_settings.MongoEventsConnectionString);

            _toRepository = new MongoTransitionRepository(_settings.MongoEventsConnectionString, "transitions_upgraded");

            _batchBlock.LinkTo(_saveBlock, new DataflowLinkOptions { PropagateCompletion = true });
        }

        private IEnumerable<Transition> RunUpgraders(Transition transition, IEnumerable<IUpgrader> upgraders)
        {
            var input = new List<Transition> { transition };
            var output = new List<Transition>();

            foreach (var upgrader in upgraders)
            {
                foreach (var t in input)
                    output.AddRange(upgrader.Upgrade(t));

                // Upgraded transitions from current IUpgrader now input to the following IUpgrader
                input = output;
                output = new List<Transition>();
            }

            return input;
        }

        public void Upgrade()
        {
            OutputWriter.WriteLine("PAQK.Upgrade");
            OutputWriter.WriteLine("---------");
            OutputWriter.WriteLine("  Events Database:");
            OutputWriter.WriteLine("      {0}", _settings.MongoEventsConnectionString);
            OutputWriter.WriteLine("---------");

            var number = GetLatestSuccesfulUpgradeNumber();

            var allUpgraders = _upgraders.ToList();
            try
            {
                CheckThatUpgradersHasCorrectUpgradeNumbers(allUpgraders);
            }
            catch (DuplicatedUpgraderNumberException)
            {
                return;
            }

            var newUpgraders = allUpgraders
                .Where(u => u.IsEnabled && u.Number > number)
                .OrderBy(u => u.Number)
                .ToList();

            if (newUpgraders.Count < 1)
            {
                OutputWriter.WriteLine("All upgraders already applied for this database.");
                OutputWriter.WriteLine("Latest successful upgrade number is {0}.", number);
                if (InputReader != null) InputReader.ReadKey();
                return;
            }

            if (!ConfirmUpgradersListForApplying(newUpgraders)) return;

            try
            {
                // Drop collection that contains previously upgraded transitions
                _events.Database.GetCollection("transitions_upgraded").Drop();

                var transitions = _fromRepository.GetTransitions();
                var stopwatch = Stopwatch.StartNew();

                int inputTransitionsCounter = 0;
                int outputTransitionsCounter = 0;
                foreach (var transition in transitions)
                {
                    if (++inputTransitionsCounter % 10000 == 0)
                        OutputWriter.WriteLine("Transitions #{0:n0}", inputTransitionsCounter);

                    var upgraded = RunUpgraders(transition, newUpgraders);

                    foreach (var trn in upgraded.Where(x => x.Events.Any()))
                    {
                        outputTransitionsCounter++;
                        SaveAsync(trn);
                    }
                }

                WaitForSaveCompletion();

                var backupCollectionName = "transitions_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // use only one connection for the following operations
                using (_events.Database.RequestStart())
                {
                    // wait untill count of transitions will be as expected
                    while (_toRepository.CountTransitions() != outputTransitionsCounter)
                        Thread.Sleep(200);

                    OutputWriter.WriteLine("Renaming collections...");
                    _events.Database.RenameCollection("transitions", backupCollectionName);
                    _events.Database.RenameCollection("transitions_upgraded", "transitions");

                    UpdateLatestSuccesfulUpgradeNumber(newUpgraders.Last().Number);
                }

                stopwatch.Stop();

                var message = String.Format("Upgraded in {0}. Total number of transitions {1:n0}/{2:n0} (initial/resulting)." + Environment.NewLine +
                    "Previous transitions collection was renamed to '{3}'.",
                    stopwatch.Elapsed.ToReadableString(), inputTransitionsCounter, outputTransitionsCounter, backupCollectionName);

                _logger.Info(message);
                OutputWriter.WriteLine(message);

                if (InputReader != null) InputReader.ReadKey();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
            }
        }

        private bool ConfirmUpgradersListForApplying(IEnumerable<IUpgrader> newUpgraders)
        {
            if (InputReader != null)
            {
                OutputWriter.WriteLine("Are you sure that you want to run the following upgraders? (type 'yes' or 'no')");
                foreach (var upgrader in newUpgraders)
                    OutputWriter.WriteLine("  {0}) {1}", upgrader.Number, upgrader.GetType().Name);
                OutputWriter.Write("> ");

                var answer = InputReader.ReadLine();
                if (String.Compare(answer, "yes", StringComparison.OrdinalIgnoreCase) != 0)
                    return false;
            }
            else
            {
                OutputWriter.WriteLine("following upgraders will be run:");
                foreach (var upgrader in newUpgraders)
                    OutputWriter.WriteLine("  {0}) {1}", upgrader.Number, upgrader.GetType().Name);
            }
            return true;
        }

        private Int32 GetLatestSuccesfulUpgradeNumber()
        {
            var upgradeDocument = _events.Upgrades.FindOne();

            if (upgradeDocument == null)
                return 0;

            return upgradeDocument.LatestSuccessfulUpgrade;
        }

        private void UpdateLatestSuccesfulUpgradeNumber(Int32 number)
        {
            _events.Upgrades.Save(new UpgradeDocument { LatestSuccessfulUpgrade = number }, WriteConcern.Acknowledged);
        }

        private void CheckThatUpgradersHasCorrectUpgradeNumbers(IEnumerable<IUpgrader> upgraders)
        {
            var dictionary = new Dictionary<Int32, IUpgrader>();

            foreach (var upgrader in upgraders)
            {
                if (dictionary.ContainsKey(upgrader.Number))
                {
                    OutputWriter.WriteLine("Upgrader {0} use number {1}, but this number is already in use by {2}",
                        upgrader.GetType().Name, upgrader.Number, dictionary[upgrader.Number].GetType().Name);

                    OutputWriter.WriteLine("Fix this problem and run upgrade again.");

                    if (InputReader != null) InputReader.ReadKey();
                    throw new DuplicatedUpgraderNumberException();
                }

                dictionary.Add(upgrader.Number, upgrader);
            }
        }

        /// <summary>
        /// Block that batchs transitions for save
        /// </summary>
        private readonly BatchBlock<Tuple<ITransitionRepository, Transition>> _batchBlock =
            new BatchBlock<Tuple<ITransitionRepository, Transition>>(1000);

        private readonly ActionBlock<Tuple<ITransitionRepository, Transition>[]> _saveBlock =
            new ActionBlock<Tuple<ITransitionRepository, Transition>[]>(tuples =>
            {
                try
                {
                    if (tuples.Length > 0)
                    {
                        var repository = tuples[0].Item1;
                        var transitions = tuples.Select(t => t.Item2);
                        repository.AppendTransitions(transitions);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex);
                }
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });

        private void SaveAsync(Transition transition)
        {
            _batchBlock.Post(new Tuple<ITransitionRepository, Transition>(_toRepository, transition));
        }

        private void WaitForSaveCompletion()
        {
            OutputWriter.WriteLine("Waiting for save completion...");
            _batchBlock.Complete();
            _batchBlock.Completion.Wait();
        }

        #endregion
    }
}