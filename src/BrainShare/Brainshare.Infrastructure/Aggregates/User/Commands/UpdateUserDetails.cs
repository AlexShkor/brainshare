using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class UpdateUserDetails: Command
    {
        public string UserName { get; set; }
    }
}