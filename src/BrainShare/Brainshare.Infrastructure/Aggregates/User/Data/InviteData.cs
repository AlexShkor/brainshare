namespace Brainshare.Infrastructure.Aggregates.User.Data
{
    public class InviteData 
    {
        public string Email { get; set; }

        public string EmailVerificationCode { get; set; }

        public string UserId { get; set; }
    }

    public enum UserRole
    {
        Administrator = 0
    }
}
