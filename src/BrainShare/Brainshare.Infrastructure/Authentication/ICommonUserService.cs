using System;
using BrainShare.Domain.Documents.Data;

namespace Brainshare.Infrastructure.Authentication
{
    public interface ICommonUserService
    {
        CommonUser GetUserByLoginServiceInfo(LoginServiceTypeEnum loginServiceType, string serviceId);
        CommonUser GetById(string id);
        object GetUserByType(Type userType, string Id);
    }
}