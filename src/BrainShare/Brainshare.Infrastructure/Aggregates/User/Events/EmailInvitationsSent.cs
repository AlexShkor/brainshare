using System.Collections.Generic;
using Brainshare.Infrastructure.Aggregates.User.Data;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class EmailInvitationsSent : Event
    {
        public List<InviteData> SendInvitesTo { get; set; }

        public string OptionalMessage { get; set; }
    }
}