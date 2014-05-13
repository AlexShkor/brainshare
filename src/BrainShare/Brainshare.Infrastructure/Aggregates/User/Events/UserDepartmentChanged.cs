using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class UserDepartmentChanged : Event
    {
        public string Department { get; set; }
    }
}
