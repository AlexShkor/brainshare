using System.Security.Principal;
using System.Web;
using BrainShare.Domain.Documents.Data;

namespace Brainshare.Infrastructure.Authentication
{
    public interface IAuthentication
    {
        HttpContext HttpContext { get; set; }
        CommonUser Login(string login, string password, bool isPersistent);
        void Logout();
        IPrincipal CurrentUser { get; }
        void LoginUser(LoginServiceTypeEnum loginServiceType, string serviceId, bool isPersistent);
    }
}
