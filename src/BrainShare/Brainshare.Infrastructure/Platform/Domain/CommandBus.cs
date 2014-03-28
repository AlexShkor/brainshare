using System;
using Brainshare.Infrastructure.Platform.Dispatching.Interfaces;
using Brainshare.Infrastructure.Platform.Domain.Interfaces;

namespace Brainshare.Infrastructure.Platform.Domain
{
    /// <summary>
    /// Messages bus for commands
    /// </summary>
    public interface ICommandBus
    {
        void Send(params ICommand[] commands);
        void Send<TCommand>(Action<TCommand> builder) where TCommand : ICommand, new();
    }

    /// <summary>
    /// Messages bus for commands
    /// </summary>
    public class CommandBus : ICommandBus
    {
        private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        private readonly IDispatcher _dispatcher;

        public CommandBus(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Send single or several messages
        /// </summary>
        public void Send(params ICommand[] commands)
        {
            PrepareCommands(commands);

            try
            {
                foreach (var command in commands)
                    _dispatcher.Dispatch(command);
            }
            catch (Exception ex)
            {
                // we are not throwing exception here, because dispatching 
                // may be performed asynchronously and on another machine
                // (but right now we dispatching synchronously)
                // so we can just log error message
                _log.Error(ex);
            }
        }

        /// <summary>
        /// Send single message using builder
        /// </summary>
        public void Send<TCommand>(Action<TCommand> builder) where TCommand : ICommand, new()
        {
            var command = new TCommand();
            builder(command);
            Send(command);
        }

        /// <summary>
        /// Prepare commands before they reach adressee
        /// </summary>
        private void PrepareCommands(params ICommand[] commands)
        {
            foreach (ICommand command in commands)
            {
                command.Metadata.CommandId = Guid.NewGuid().ToString();
                command.Metadata.CreatedDate = DateTime.UtcNow;
                command.Metadata.TypeName = command.GetType().FullName;
                command.Metadata.CommandId = Guid.NewGuid().ToString();
            }
        }
    }
}