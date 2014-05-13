using System.Collections.Generic;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Registration.Commands
{
    public class FinalizeUserRegistation : Command
    {
        public string UserEmail { get; set; }

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> Expertises { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string TenantName { get; set; }

        public bool IsPublicEmail { get; set; }
    }
}
