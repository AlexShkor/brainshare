using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class UserFirstLastNameChanged : Event
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
