using Brainshare.Infrastructure.Aggregates.User.Data;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class UserRoleAdded : Event
    {
        public UserRole Role { get; set; }
    }
}