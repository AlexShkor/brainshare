using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class ChangePassword: Command
    {
        public string NewPasswordHash { get; set; }

        public string NewSalt { get; set; }
    }
}