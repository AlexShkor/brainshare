using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class ChangePassword: Command
    {
        public string NewPassword { get; set; }
        public bool IsChangedByAdmin { get; set; }
    }
}