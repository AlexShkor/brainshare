using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class ExpertsConnected : Event
    {
        public string ConnectToUserId { get; set; }

        public string UserId { get; set; }

        public string SystemUserId { get; set; }

    }
}