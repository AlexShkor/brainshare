using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class ExpertFollowed : Event
    {
        public string FollowedUserId { get; set; }

        public string FollowerUserId { get; set; }


    }
}