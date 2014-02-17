using System.Security.Principal;
using System.Web;
using BrainShare.Domain.Documents.Data;

namespace Brainshare.Infrastructure.Authentication
{
    public interface IAuthentication
    {
        bool Login(string login, string password, bool isPersistent);
        void Logout();
        IPrincipal CurrentUser { get; }
        void LoginFb(string fbId);
        void LoginVk(string vkId);
    }
}
