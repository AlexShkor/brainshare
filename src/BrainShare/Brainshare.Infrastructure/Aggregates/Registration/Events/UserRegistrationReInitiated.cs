using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Registration.Events
{
    public class UserRegistrationReInitiated : Event
    {
        public string Email { get; set; }

        public string UserId { get; set; }

        public string InviteOptionalMessage { get; set; }

        public string TenantName { get; set; }

        public bool IsInitiatedFromInvite { get; set; }

        public string EmailVerificationCode { get; set; }
    }
}
