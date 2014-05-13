using Brainshare.Infrastructure.Aggregates.User.Data;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class RemoveUserRole : Command
    {
        public UserRole Role { get; set; }
    }
}