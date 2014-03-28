using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using Brainshare.Infrastructure.Databases;
using Brainshare.Infrastructure.Platform.Dispatching.Interfaces;
using Brainshare.Infrastructure.Platform.Domain.Interfaces;
using Brainshare.Infrastructure.Platform.Domain.Transitions.Interfaces;
using Brainshare.Infrastructure.Platform.Output;
using NLog;
using StructureMap.Attributes;
using Uniform;

namespace Brainshare.Infrastructure.Platform.Replay
{
    public class Replayer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [SetterProperty]
        public ITransitionRepository TransitionRepository { get; set; }

        [SetterProperty]
        public IDispatcher Dispatcher { get; set; }

        [SetterProperty]
        public MongoViewDatabase MongoView { get; set; }

        [SetterProperty]
        public Brainshare.Infrastructure.Settings.Settings Settings { get; set; }

        [SetterProperty]
        public UniformDatabase UniformDatabase { get; set; }

        [SetterProperty]
        public IOutputWriter OutputWriter { get; set; }

        public IInputReader InputReader
        {
            get { return OutputWriter as IInputReader; }
        }

        public void Start()
        {
            OutputWriter.WriteLine("PAQK.Reply");
            OutputWriter.WriteLine("---------");
            OutputWriter.WriteLine("  Events Database:");
            OutputWriter.WriteLine("      {0}", Settings.MongoEventsConnectionString);
            OutputWriter.WriteLine("  View Database:");
            OutputWriter.WriteLine("      {0}", Settings.MongoViewConnectionString);
            

            if (InputReader != null)
            {
                OutputWriter.WriteLine();
                OutputWriter.WriteLine();
                OutputWriter.WriteLine("Are you sure that you want to drop these databases? (type 'yes' or 'no')");
                OutputWriter.WriteLine("   {0}", Settings.MongoViewConnectionString);

                OutputWriter.Write("> ");

                var answer = InputReader.ReadLine();
                if (String.Compare(answer, "yes", StringComparison.OrdinalIgnoreCase) != 0)
                    return;
            }


            try
            {
                BeforeReplayPreparations();
                UniformDatabase.EnterInMemoryMode();

                var transitions = TransitionRepository.GetTransitions();
                var stopwatch = Stopwatch.StartNew();

                var counter = 0;
                foreach (var transition in transitions)
                {
                    foreach (var evnt in transition.Events)
                    {
                        if (++counter % 10000 == 0)
                            OutputWriter.WriteLine("Events #{0:n0}", counter);

                        DispatchAsync((IEvent) evnt.Data);
                    }
                }

                OutputWriter.WriteLine("Waiting for dispatch completion...");
                WaitForDispatchCompletion();

                OutputWriter.WriteLine("Flushing...");
                UniformDatabase.LeaveInMemoryMode(true);
                stopwatch.Stop();
                OutputWriter.WriteLine("Flush completed");

                var message = String.Format("Replayed in {0}. Total number of events {1:n0}", 
                    stopwatch.Elapsed.ToReadableString(), counter);
                
                logger.Info(message);
                OutputWriter.WriteLine(message);

                if (InputReader != null)
                    InputReader.ReadKey();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
                OutputWriter.WriteLine(ex.Message);
            }
        }

        private void BeforeReplayPreparations()
        {
            // MongoDB (can't drop entire database - Mongo Lab limitation)
            var mongoCollections = MongoView.Database.GetCollectionNames().Where(x => !x.StartsWith("system."));
            foreach (var collection in mongoCollections)
            {
                MongoView.Database.DropCollection(collection);
            }
        }

        private readonly ActionBlock<Tuple<IDispatcher, IEvent>> _dispatchBlock =
            new ActionBlock<Tuple<IDispatcher, IEvent>>(tuple =>
            {
                try
                {
                    var dispatcher = tuple.Item1;
                    var evnt = tuple.Item2;

                    dispatcher.Dispatch(evnt);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex);
                }
            }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 1 });

        private void DispatchAsync(IEvent evnt)
        {
            _dispatchBlock.Post(new Tuple<IDispatcher, IEvent>(Dispatcher, evnt));
        }

        public void WaitForDispatchCompletion()
        {
            _dispatchBlock.Complete();
            _dispatchBlock.Completion.Wait();
        }
    }

    internal static class TimeSpanEntenstion
    {
        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}{4}",
                span.Days > 0 ? string.Format("{0:0} days, ", span.Days) : string.Empty,
                span.Hours > 0 ? string.Format("{0:0} hours, ", span.Hours) : string.Empty,
                span.Minutes > 0 ? string.Format("{0:0} minutes, ", span.Minutes) : string.Empty,
                span.Seconds > 0 ? string.Format("{0:0} seconds, ", span.Seconds) : string.Empty,
                span.Milliseconds > 0 ? string.Format("{0:0} ms", span.Milliseconds) : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            return formatted;
        }
    }
}