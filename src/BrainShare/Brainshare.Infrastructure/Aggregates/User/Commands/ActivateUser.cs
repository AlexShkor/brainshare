using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class ActivateUser : Command
    {
        public string Email { get; set; }
    }
}