using System;
using System.Security.Principal;
using BrainShare.Domain.Documents.Data;

namespace Brainshare.Infrastructure.Authentication
{
    public class UserProvider : IPrincipal
    {

        public UserProvider(LoginServiceTypeEnum loginServiceType, string serviceId, ICommonUserService commonUserService)
        {
            userIdentity = new UserIdentity();

            userIdentity.Init(loginServiceType, serviceId, commonUserService);
        }


        private UserIdentity userIdentity { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity { get { return userIdentity; } }

        public override string ToString()
        {
            return userIdentity.Name;
        }

    }
}