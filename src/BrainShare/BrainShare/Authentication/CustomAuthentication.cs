using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using BrainShare.Documents;
using BrainShare.Mongo;
using BrainShare.Services;
using BrainShare.Utilities;

namespace BrainShare.Authentication
{
    public class CustomAuthentication : IAuthentication
    {
        private readonly UsersService _users;
        private readonly ShellUserService _shellUsers;

        public CustomAuthentication(UsersService users,ShellUserService shellUsers)
        {
            _users = users;
            _shellUsers = shellUsers;
        }

        private const string CookieName = "__AUTH_COOKIE";

        public HttpContext HttpContext
        {
            get { return _httpContext; }
            set { _httpContext = value; }
        }

        public User Login(string email, string password, bool isPersistent)
        {
            var retUser = _users.GetByCredentials(email, password);

            if (retUser == null)
            {
                var shellUser = _shellUsers.GetByCredentials(email, password);

                if (shellUser != null)
                {
                    CreateCookie(email, isPersistent, Constants.ShellUserFlag);
                }

                retUser = shellUser.MapShellUser();
            }

            return retUser;
        }

        public User Login(string email)
        {
            var retUser = _users.GetUserByEmail(email);
            if (retUser != null)
            {
                CreateCookie(email);
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
                            _currentUser = new UserProvider(ticket.Name, _users,_shellUsers, ticket.UserData);
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

        public void LoginUser(User user, bool isPersistent)
        {
            CreateCookie(user.Email,isPersistent);
        }
    }
}