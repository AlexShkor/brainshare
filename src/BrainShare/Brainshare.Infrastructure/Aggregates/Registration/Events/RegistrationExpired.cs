using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Registration.Events
{
    public class RegistrationExpired : Event
    {
        public string Email { get; set; }

        public string UserId { get; set; }
    }
}
