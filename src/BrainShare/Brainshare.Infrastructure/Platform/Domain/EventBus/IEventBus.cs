using System.Collections.Generic;
using Brainshare.Infrastructure.Platform.Domain.Interfaces;

namespace Brainshare.Infrastructure.Platform.Domain.EventBus
{
    public interface IEventBus
    {
        void Publish(IEvent eventMessage);
        void Publish(IEnumerable<IEvent> eventMessages);
    }
}