using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using BrainShare.Domain.Documents.Data;
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
            var retUser = _commonUserService.GetUserByLoginServiceInfo(LoginServiceTypeEnum.Email, email);

            if (retUser == null)
            {
                return null;
            }

            if (retUser.LoginServices.Any(e => e.LoginType == LoginServiceTypeEnum.Email &&  e.ServiceUserId == email && e.AccessToken == _cryptoHelper.GetPasswordHash(password, e.Salt)))
            {
                CreateCookie(LoginServiceTypeEnum.Email, email, isPersistent);
                return retUser;
            }

            return null;
        }

        public CommonUser Login(LoginServiceTypeEnum loginServiceType, string serviceId)
        {
            var retUser = _commonUserService.GetUserByLoginServiceInfo(loginServiceType,serviceId);
            if (retUser != null)
            {
                CreateCookie(loginServiceType,serviceId);
            }

            return retUser;
        }

        private void CreateCookie(LoginServiceTypeEnum loginServiceType, string serviceId, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                serviceId,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                ((int)loginServiceType).ToString(),
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
                            _currentUser = new UserProvider((LoginServiceTypeEnum)int.Parse(ticket.UserData),ticket.Name, _commonUserService);
                        }

                        else
                        {
                            _currentUser = new UserProvider(LoginServiceTypeEnum.Email, null,null);
                        }
                    }

                    catch(Exception)
                    {
                        _currentUser = new UserProvider(LoginServiceTypeEnum.Email, null, null);
                    }
                }

                return _currentUser;
            }
        }

        public void LoginUser(LoginServiceTypeEnum loginServiceType, string serviceId,  bool isPersistent)
        {
            CreateCookie(loginServiceType,serviceId,isPersistent);
        }
    }
}