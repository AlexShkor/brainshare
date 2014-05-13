using System.Collections.Generic;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Registration.Events
{
    public class RegistrationUserCreated : Event
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> Expertises { get; set; }
    }
}
