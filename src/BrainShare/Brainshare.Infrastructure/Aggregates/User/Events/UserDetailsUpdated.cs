using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class UserDetailsUpdated: Event
    {
        public string UserName { get; set; }
    }
}