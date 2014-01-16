using System;
using BrainShare.Domain.Documents.Data;

namespace Brainshare.Infrastructure.Authentication
{
    public interface ICommonUserService
    {
        CommonUser GetUserByLoginServiceInfo(LoginServiceTypeEnum loginServiceType, string serviceId);
        CommonUser GetUserByEmail(string email);
        CommonUser GetUserByVkId(string id);
        CommonUser GetUserByFacebookId(string id);
        CommonUser GetById(string id);
        object GetUserByType(Type userType, string Id);
    }
}