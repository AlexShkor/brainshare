using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Models;
using BrainShare.Services;
using BrainShare.ViewModels;
using Facebook;
using MongoDB.Bson;

namespace BrainShare.Controllers
{
    public class UserController : Controller
    {
        private readonly UsersService _users;
        private readonly Settings _settings;
        public IAuthentication Auth { get; set; }



        private readonly FacebookClient _fb;

        public string FacebookCallbackUri
        {
            get
            {
                return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "") +
                                  Url.Action("FacebookCallback");
            }
        }


        public UserController(IAuthentication auth , UsersService users,  FacebookClientFactory fbFactory, Settings settings)
        {
            _users = users;
            _settings = settings;
            Auth = auth;
            _fb = fbFactory.GetClient();
        }

        private User CurrentUser
        {
            get { return ((IUserProvider)Auth.CurrentUser.Identity).User; }
        }

        public ActionResult UserLogin()
        {
            return View(CurrentUser);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginView());
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Email, loginView.Password, loginView.IsPersistent);
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState["Password"].Errors.Add("Such e-mail or password are not registered");
            }
            return View(loginView);
        }

        public ActionResult Logout()
        {
            Auth.Logout();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
            }
            return View(model);
        }


        [FacebookAuthorize]

        public ActionResult ProcessFacebook(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                Session.Clear();
            }
            dynamic fbUser = _fb.Get("me");
            var user = _users.GetByFacebookId((string)fbUser.id);
            if (user == null)
            {
                user = new User
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Email = fbUser.email,
                    FacebookId = fbUser.id,
                    FirstName = fbUser.first_name,
                    LastName = fbUser.last_name,
                };
                _users.Save(user);
            }
            Auth.LoginUser(user, true);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoginWithFacebook(string returnUrl)
        {
            var csrfToken = Guid.NewGuid().ToString();
            Session[SessionKeys.FbCsrfToken] = csrfToken;
            var state = Convert.ToBase64String(Encoding.UTF8.GetBytes(_fb.SerializeJson(new { returnUrl = returnUrl, csrf = csrfToken })));
            const string scope = "user_about_me,email";
            var fbLoginUrl = _fb.GetLoginUrl(
                new
                {
                    client_id = _settings.FacebookAppId,
                    client_secret = _settings.FacebookSecretKey,
                    redirect_uri = FacebookCallbackUri,
                    response_type = "code",
                    scope = scope,
                    state = state
                });
            return Redirect(fbLoginUrl.AbsoluteUri);
        }

        private ActionResult RedirectToProcessFacebook(string returnUrl = null)
        {
            return RedirectToAction("ProcessFacebook", new { returnUrl });
        }

        public ActionResult FacebookCallback(string code, string state)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
                return RedirectToProcessFacebook();

            // first validate the csrf token
            dynamic decodedState;
            try
            {
                decodedState = _fb.DeserializeJson(Encoding.UTF8.GetString(Convert.FromBase64String(state)), null);
                var exepectedCsrfToken = Session[SessionKeys.FbCsrfToken] as string;
                // make the fb_csrf_token invalid
                Session[SessionKeys.FbCsrfToken] = null;

                if (!(decodedState is IDictionary<string, object>) || !decodedState.ContainsKey("csrf") || string.IsNullOrWhiteSpace(exepectedCsrfToken) || exepectedCsrfToken != decodedState.csrf)
                {
                    return RedirectToProcessFacebook();
                }
            }
            catch
            {
                // log exception
                return RedirectToProcessFacebook();
            }

            try
            {
                dynamic result = _fb.Post("oauth/access_token",
                                          new
                                          {
                                              client_id = _settings.FacebookAppId,
                                              client_secret = _settings.FacebookSecretKey,
                                              redirect_uri = FacebookCallbackUri,
                                              code = code
                                          });

                Session[SessionKeys.FbAccessToken] = result.access_token;
                if (result.ContainsKey("expires"))
                    Session[SessionKeys.FbExpiresIn] = DateTime.Now.AddSeconds(result.expires);

                if (decodedState.ContainsKey("returnUrl"))
                {
                    return RedirectToProcessFacebook(decodedState.returnUrl);
                }

                return RedirectToProcessFacebook();
            }
            catch
            {
                // log exception
                return RedirectToProcessFacebook();
            }
        }
    }

    public class Settings
    {
        public string FacebookAppId
        {
            get { return "366146963495815"; }
        }

        public string FacebookSecretKey
        {
            get { return "dddbde39b505a7186604dbf208a2c715"; }
        }
    }


    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class FacebookAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var accessToken = httpContext.Session[SessionKeys.FbAccessToken] as string;
            if (string.IsNullOrWhiteSpace(accessToken))
                return false;
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/account/loginwithfacebook");
        }
    }
}

