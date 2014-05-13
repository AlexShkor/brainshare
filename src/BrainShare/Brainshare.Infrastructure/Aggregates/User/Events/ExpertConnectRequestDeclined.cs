using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class ExpertConnectRequestDeclined : Event
    {
        public string ConnectUserId { get; set; }

        public string UserId { get; set; }

    }
}