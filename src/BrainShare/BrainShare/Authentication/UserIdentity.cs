using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using BrainShare.Documents;
using BrainShare.Services;
using BrainShare.Utilities;

namespace BrainShare.Authentication
{
    public class UserIdentity : IIdentity, IUserProvider
    {
        public CommonUser User { get; set; }

        public string AuthenticationType
        {
            get
            {
                return typeof(User).ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.Email;
                }

                return "Anonym";
            }
        }

        public void Init (string email, ICommonUserService usersService)
        {
            if (!string.IsNullOrEmpty(email))
            {
                User = usersService.GetUserByEmail(email);
            }
        }
    }
}