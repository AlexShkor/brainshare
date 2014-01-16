using System.Security.Principal;
using BrainShare.Domain.Documents.Data;

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

        public void Init(LoginServiceTypeEnum loginServiceType, string serviceId, ICommonUserService usersService)
        {
            if (serviceId != null && loginServiceType != null)
            {
                  User = usersService.GetUserByLoginServiceInfo(loginServiceType,serviceId);
            }
           
        }
    }
}