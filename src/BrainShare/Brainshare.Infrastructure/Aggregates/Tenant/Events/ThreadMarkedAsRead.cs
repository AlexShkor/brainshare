using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Tenant.Events
{
    public class ThreadMarkedAsRead : Event
    {
        public string UserId { get; set; }
    }
}
