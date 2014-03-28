using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class PasswordChanged : Event
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool WasChangedByAdmin { get; set; }
    }
}