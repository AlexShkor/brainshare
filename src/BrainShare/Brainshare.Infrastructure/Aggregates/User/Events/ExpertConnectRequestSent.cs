using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class ExpertConnectRequestSent : Event
    {
        public string ConnectToUserId { get; set; }

        public string UserId { get; set; }

    }
}