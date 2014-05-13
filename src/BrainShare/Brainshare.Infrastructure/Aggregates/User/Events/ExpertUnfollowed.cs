using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class ExpertUnfollowed : Event
    {
        public string FollowingUserId { get; set; }

        public string UserId { get; set; }

        public bool Silent { get; set; }
    }
}