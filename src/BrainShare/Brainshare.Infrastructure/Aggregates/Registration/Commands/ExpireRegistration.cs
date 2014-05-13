using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Registration.Commands
{
    public class ExpireRegistration : Command
    {
        public string Email { get; set; }
    }
}
