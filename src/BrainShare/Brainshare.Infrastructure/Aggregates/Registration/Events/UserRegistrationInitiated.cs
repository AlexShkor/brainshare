using System;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Registration.Events
{
    public class UserRegistrationInitiated : Event
    {
        public string Email { get; set; }

        public string EmailVerificationCode { get; set; }

        public string UserId { get; set; }

        public bool IsInitiatedFromInvite { get; set; }

        public string InviteOptionalMessage { get; set; }

        public string TenantName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string InviteFromDomain { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
