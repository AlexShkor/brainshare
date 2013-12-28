using System.Security.Principal;

namespace Brainshare.Infrastructure.Authentication
{
    public class UserIdentity : IIdentity, IUserProvider
    {
        public CommonUser User { get; set; }

        public string AuthenticationType
        {
            get
            {
                return typeof(CommonUser).ToString();
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