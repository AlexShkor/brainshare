using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class UserDeactivated : Event
    {
        public string Email { get; set; }
    }
}