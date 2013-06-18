using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Facebook;
using BrainShare.Services;
using BrainShare.ViewModels;
using Facebook;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Linq;

namespace BrainShare.Controllers
{

    public class UserController : BaseController
    {
        private readonly UsersService _users;
        private readonly Settings _settings;
        public IAuthentication Auth { get; set; }

        private readonly FacebookClient _fb;

        public string FacebookCallbackUri
        {
            get
            {
                var url = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");
                if (Request.Url.Host != "localhost")
                {
                    url = url.Replace(":" + Request.Url.Port, "");
                }
                return url + Url.Action("FacebookCallback");
            }
        }


        public UserController(IAuthentication auth, UsersService users, FacebookClientFactory fbFactory, Settings settings)
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

                ModelState["Password"].Errors.Add("Such e-mail or password is not registered");
            }
            return View(loginView);
        }

        public ActionResult Logout()
        {
            Auth.Logout();
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var anyUser = _users.GetUserByEmail(model.Email);
                if (anyUser == null)
                {
                    var newUser = new User()
                                      {
                                          Id = GetIdForUser(),
                                          FirstName = model.FirstName,
                                          LastName = model.LastName,
                                          Email = model.Email,
                                          Password = model.Password
                                      };

                    _users.AddUser(newUser);

                    var mailer = new MailService();
                    var welcomeEmail = mailer.SendWelcomeMessage(newUser);
                    welcomeEmail.Deliver();

                    var login = new LoginView()
                                    {
                                        Email = newUser.Email,
                                        Password = newUser.Password
                                    };

                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ModelState.AddModelError("Email", "Пользователь с таким e-mail уже существует");
                }
            }
            return View(model);
        }

        public static string GetIdForUser()
        {
            return ObjectId.GenerateNewId().ToString();
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
            Session["FacebookId"] = user.FacebookId; //
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

        public ActionResult GetFbFriends()
        {

            //var user1 = _users.GetById("51a9cd852c7e6e15bc73e1c0");
            //var user3 = _users.GetById(UserId);

            if (Session["FacebookId"] != null)
            {
                dynamic fbresult = _fb.Get("fql", new { q = "SELECT uid, first_name, last_name, pic_square FROM user WHERE uid in (SELECT uid2 FROM friend WHERE uid1 = me())" });

                var fbfriendsInfo = fbresult["data"].ToString();
                List<FacebookFriend> fbFriends = JsonConvert.DeserializeObject<List<FacebookFriend>>(fbfriendsInfo);

                var fbIds = fbFriends.Select(x => x.FacebookId);

                var existingFbIds = _users.GetExistingFacebookIds(fbIds);

                var friends = fbFriends.Where(friend => existingFbIds.Contains(friend.FacebookId)).ToList();
                
                foreach (var friend in friends)
                {
                    friend.Id = _users.GetByFacebookId(friend.FacebookId).Id;
                }

                var model = new FacebookSelectorViewModel(friends);

                return View(model);
            }

            return null;
        }
    }
}

