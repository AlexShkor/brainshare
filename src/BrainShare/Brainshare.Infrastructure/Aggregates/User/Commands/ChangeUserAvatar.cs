using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class ChangeUserAvatar : Command
    {
        public string AvatarImageId { get; set; }
    }
}
