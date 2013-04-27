using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using BrainShare.Documents;
using BrainShare.Mongo;
using BrainShare.Services;

namespace BrainShare.Authentication
{
    public class CustomAuthentication : IAuthentication
    {
        private readonly UsersService _users;

        public CustomAuthentication(UsersService users)
        {
            _users = users;
        }

        private const string CookieName = "__AUTH_COOKIE";

        public HttpContext HttpContext { get; set; }

        public User Login(string email, string password, bool isPersistent)
        {
            var retUser = _users.GetByCredentials(email, password);
            if (retUser != null)
            {
                CreateCookie(email, isPersistent);
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

        private void CreateCookie(string email, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                email,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                string.Empty,
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
                            _currentUser = new UserProvider(ticket.Name, _users);
                        }

                        else
                        {
                            _currentUser = new UserProvider(null, null);
                        }
                    }

                    catch(Exception ex)
                    {
                        _currentUser = new UserProvider(null, null);
                    }
                }

                return _currentUser;
            }
        }


    }
}