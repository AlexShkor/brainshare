using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Registration.Commands
{
    public class InitiateUserRegisteration : Command
    {
        public string Email { get; set; }

        public string UserId { get; set; }

        public bool IsInitiatedFromInvite { get; set; }

        public string InviteOptionalMessage { get; set; }

        public string InviteFromDomain { get; set; }
    }
}
