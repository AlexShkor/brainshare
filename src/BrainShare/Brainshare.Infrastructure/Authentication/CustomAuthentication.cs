using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Brainshare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Services;

namespace Brainshare.Infrastructure.Authentication
{
    public class CustomAuthentication : IAuthentication
    {
        private readonly UsersService _users;
        private readonly ICommonUserService _commonUserService;
        private readonly CryptographicHelper _cryptoHelper;

        public CustomAuthentication(UsersService users, ICommonUserService commonUserService, CryptographicHelper cryptoHelper)
        {
            _users = users;
            _commonUserService = commonUserService;
            _cryptoHelper = cryptoHelper;
        }

        private const string CookieName = "__AUTH_COOKIE";

        public HttpContext HttpContext
        {
            get { return HttpContext.Current; }
        }

        public bool Login(string email, string password, bool isPersistent)
        {
            var user = _commonUserService.GetUserByEmail(email);
            if (user != null && user.Password == _cryptoHelper.GetPasswordHash(password, user.Salt))
            {
                LoginUser(user.Id, true);
                return true;
            }
            return false;
        }

        public void LoginVk(string id)
        {
            var user = _commonUserService.GetUserByVkId(id);
            if (user != null)
            {
                LoginUser(user.Id, true);
            }
        }

        public void LoginFb(string id)
        {
            var user = _commonUserService.GetUserByFacebookId(id);
            if (user != null)
            {
               LoginUser(user.Id,true);
            }
        }
        

        public void Logout()
        {
            var httpCookie = HttpContext.Response.Cookies[CookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private IPrincipal _currentUser;

        private const int CURRENT_TICKET_VERSION = 2;

        public IPrincipal CurrentUser
        { 
            get
            {
                if (_currentUser == null)
                {
                        HttpCookie authCookie = HttpContext.Request.Cookies.Get(CookieName);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            if (ticket != null)
                            {
                                if (ticket.Version == CURRENT_TICKET_VERSION)
                                {
                                    _currentUser = new UserProvider(ticket.Name, ticket.UserData);
                                }
                                else
                                {
                                    FormsAuthentication.SignOut();
                                }
                            }
                        }
                }
                return _currentUser;
            }
        }

        private void LoginUser(string userId, bool isPersistent)
        {
            var user = _users.GetById(userId);
            var data = user.FullName;
            var ticket = new FormsAuthenticationTicket(
                CURRENT_TICKET_VERSION,
                userId,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                data,
                FormsAuthentication.FormsCookiePath);

            // Encrypt ticket
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie
            var authCookie = new HttpCookie(CookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(authCookie);
        }
    }
}