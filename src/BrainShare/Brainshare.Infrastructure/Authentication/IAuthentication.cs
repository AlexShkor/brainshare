using System.Security.Principal;
using System.Web;

namespace Brainshare.Infrastructure.Authentication
{
    public interface IAuthentication
    {
        HttpContext HttpContext { get; set; }
        CommonUser Login(string login, string password, bool isPersistent);
        CommonUser Login(string login);
        void Logout();
        IPrincipal CurrentUser { get; }
        void LoginUser(string email, bool isPersistent);
    }
}
