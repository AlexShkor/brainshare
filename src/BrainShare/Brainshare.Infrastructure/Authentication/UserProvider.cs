using System;
using System.Security.Principal;
using BrainShare.Domain.Documents.Data;

namespace Brainshare.Infrastructure.Authentication
{
    public class UserProvider : IPrincipal
    {
        public UserProvider(string userId, string userName)
        {
            Identity = new UserIdentity(userId, userName);

        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public IIdentity Identity { get; private set; }

        public override string ToString()
        {
            return Identity.Name;
        }

    }
}