using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class ResendEmailConfirmationEmail : Command
    {
        public string Email { get; set; }

        public string BaseUrl { get; set; }

        public string EmailVerificationCode { get; set; }
    }
}
