using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class UserDeleted: Event
    {
        public string DeletedByUserId { get; set; }
    }
}