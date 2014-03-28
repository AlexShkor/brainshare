using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class DeleteUser: Command
    {
        public string DeletedByUserId { get; set; }
    }
}