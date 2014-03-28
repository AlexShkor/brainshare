using Brainshare.Infrastructure.Aggregates.User.Events;
using Brainshare.Infrastructure.Platform.Domain;

namespace Brainshare.Infrastructure.Aggregates.User
{
    public sealed class UserState: AggregateState
    {
        public string Id { get; private set; }
        public bool UserWasDeleted { get; set; }

        public UserState()
        {
            On((UserCreated e) => Id = e.Id);
            On((UserDeleted e) => UserWasDeleted = true);
        }
    }
}