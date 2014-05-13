namespace Brainshare.Infrastructure.Aggregates.Registration.Commands
{
    public class DeleteUserRegistationWithoutContent : Command
    {
        public string Email { get; set; }
    }
}
