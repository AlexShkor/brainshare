using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using BrainShare.Infostructure;
using BrainShare.Services;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Services;

namespace BrainShare.Authentication
{
    public class CustomAuthentication : IAuthentication
    {
        private readonly UsersService _users;
        private readonly ShellUserService _shellUsers;
        private readonly ICommonUserService _commonUserService;
        private readonly CryptographicHelper _cryptoHelper;

        public CustomAuthentication(UsersService users, ShellUserService shellUsers, ICommonUserService commonUserService, CryptographicHelper cryptoHelper)
        {
            _users = users;
            _shellUsers = shellUsers;
            _commonUserService = commonUserService;
            _cryptoHelper = cryptoHelper;
        }

        private const string CookieName = "__AUTH_COOKIE";

        public HttpContext HttpContext
        {
            get { return _httpContext; }
            set { _httpContext = value; }
        }

        public CommonUser Login(string email, string password, bool isPersistent)
        {
            var retUser = _commonUserService.GetUserByEmail(email.ToLower());

            if (retUser == null)
            {
                return null;
            }

            password = _cryptoHelper.GetPasswordHash(password, retUser.Salt);


            if (retUser.Password.Equals(password))
            {
                CreateCookie(email.ToLower(), isPersistent, Constants.ShellUserFlag);
                return retUser;
            }

            return null;
        }

        public CommonUser Login(string email)
        {
            var retUser = _commonUserService.GetUserByEmail(email.ToLower());
            if (retUser != null)
            {
                CreateCookie(email.ToLower());
            }

            return retUser;
        }

        private void CreateCookie(string email, bool isPersistent = false, string userData = "")
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                email,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                userData,
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

        public void Logout()
        {
            var httpCookie = HttpContext.Response.Cookies[CookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private IPrincipal _currentUser;
        private HttpContext _httpContext = HttpContext.Current;

        public IPrincipal CurrentUser
        { 
            get
            {
                if (_currentUser == null)
                {
                    try
                    {
                        HttpCookie authCookie = HttpContext.Request.Cookies.Get(CookieName);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            _currentUser = new UserProvider(ticket.Name, _commonUserService, ticket.UserData);
                        }

                        else
                        {
                            _currentUser = new UserProvider(null, null,null);
                        }
                    }

                    catch(Exception)
                    {
                        _currentUser = new UserProvider(null, null,null);
                    }
                }

                return _currentUser;
            }
        }

        public void LoginUser(string email, bool isPersistent)
        {
            CreateCookie(email.ToLower(),isPersistent);
        }
    }
}