using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class ResetPasswordCodeUpdated : Event
    {
        public string Code { get; set; }
    }
}
