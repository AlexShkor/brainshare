using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BrainShare.Documents;

namespace BrainShare.Authentication
{
    public interface IAuthentication
    {
        HttpContext HttpContext { get; set; }
        User Login(string login, string password, bool isPersistent);
        User Login(string login);
        void Logout();
        IPrincipal CurrentUser { get; }
        void LoginUser(User user, bool isPersistent);
    }
}
