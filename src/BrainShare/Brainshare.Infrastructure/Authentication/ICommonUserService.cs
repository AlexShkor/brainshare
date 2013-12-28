using System;

namespace Brainshare.Infrastructure.Authentication
{
    public interface ICommonUserService
    {
        CommonUser GetUserByEmail(string email);
        CommonUser GetById(string id);
        object GetUserByType(Type userType, string Id);
    }
}