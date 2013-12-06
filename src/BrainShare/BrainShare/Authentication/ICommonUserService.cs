using System;

namespace BrainShare.Authentication
{
    public interface ICommonUserService
    {
        CommonUser GetUserByEmail(string email);
        CommonUser GetById(string id);
        object GetUserByType(Type userType, string Id);
    }
}