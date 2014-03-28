using System.Security.Principal;
using BrainShare.Utils.Extensions;

namespace Brainshare.Infrastructure.Authentication
{
    public class UserIdentity : IIdentity
    {
        private readonly string _userId;
        private readonly string _userName;

        public UserIdentity(string userId, string userName)
        {
            _userId = userId;
            _userName = userName;
        }

        public string AuthenticationType
        {
            get
            {
                return "Forms";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _userId.HasValue();
            }
        }

        public string Id
        {
            get
            {
                return _userId;
            }
        }

        public string Name
        {
            get
            {
                return _userName;
            }
        }

    }
}