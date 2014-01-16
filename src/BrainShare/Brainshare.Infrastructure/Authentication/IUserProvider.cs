namespace Brainshare.Infrastructure.Authentication
{
    public interface IUserProvider
    {
        CommonUser User { get; set; }
    }
}