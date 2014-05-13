using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class ExpertsDisconnected : Event
    {
        public string DisconectFromUserId { get; set; }

        public string UserId { get; set; }

        public bool Silent { get; set; }
    }
}