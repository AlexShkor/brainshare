using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Registration.Commands
{
    public class DeleteUserRegistation : Command
    {
        public string Email { get; set; }

        public bool DeleteContent { get; set; }
    }
}
