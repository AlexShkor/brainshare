using System;
using System.Security.Principal;

namespace Brainshare.Infrastructure.Authentication
{
    public class UserProvider : IPrincipal
    {

        public UserProvider(string name, ICommonUserService commonUserService, string userData = "")
        {
            userIdentity = new UserIdentity();

            userIdentity.Init(name, commonUserService);
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