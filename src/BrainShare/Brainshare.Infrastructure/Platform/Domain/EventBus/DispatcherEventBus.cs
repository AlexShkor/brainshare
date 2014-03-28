using System.Collections.Generic;
using Brainshare.Infrastructure.Platform.Dispatching.Interfaces;
using Brainshare.Infrastructure.Platform.Domain.Interfaces;

namespace Brainshare.Infrastructure.Platform.Domain.EventBus
{
    public class DispatcherEventBus : IEventBus
    {
        private readonly IDispatcher _dispatcher;

        public DispatcherEventBus(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Publish(IEvent eventMessage)
        {
            _dispatcher.Dispatch(eventMessage);
        }

        public void Publish(IEnumerable<IEvent> eventMessages)
        {
            foreach (var evnt in eventMessages)
                _dispatcher.Dispatch(evnt);
        }
    }
}