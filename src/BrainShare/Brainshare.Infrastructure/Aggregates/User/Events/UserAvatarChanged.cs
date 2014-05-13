using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class UserAvatarChanged : Event
    {
        public string AvatarImageId { get; set; }
    }
}
