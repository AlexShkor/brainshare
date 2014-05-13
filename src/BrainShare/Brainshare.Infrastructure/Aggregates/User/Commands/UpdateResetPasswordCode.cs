using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class UpdateResetPasswordCode : Command
    {
        public string Code { get; set; }
    }
}