namespace BrainShare.Authentication
{
    public interface ICommonUserService
    {
        CommonUser GetByCredentials(string email, string password);
        CommonUser GetUserByEmail(string email);
        CommonUser GetById(string id);
    }
}