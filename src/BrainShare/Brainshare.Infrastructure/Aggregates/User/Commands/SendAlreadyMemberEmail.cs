using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class SendAlreadyMemberEmail : Command
    {
        public string Email { get; set; }

        public string BaseUrl { get; set; }
    }
}
