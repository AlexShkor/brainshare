using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Tenant.Commands
{
    public class MarkThreadAsRead : Command
    {
        public string UserId { get; set; }
    }
}
