﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Metadata.Edm;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Documents.Data;
using BrainShare.Extensions;
using BrainShare.Facebook;
using BrainShare.Facebook.Dto;
using BrainShare.Infostructure;
using BrainShare.Services;
using BrainShare.Utilities;
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
        private readonly ShellUserService _shellUserService;

        public string FacebookCallbackUri
        {
            get
            {
                return UrlUtility.ApplicationBaseUrl + Url.Action("FacebookCallback");
            }
        }

        public UserController(IAuthentication auth, UsersService users,ShellUserService shellUserService, FacebookClientFactory  fbFactory, Settings settings)
        {
            _users = users;
            _settings = settings;
            _shellUserService = shellUserService;
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

                ModelState["Password"].Errors.Add("Такой e-mail или пароль не зарегистрирован");
            }
            return View(loginView);
        }

        public ActionResult Logout()
        {
            // does we need?
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
                                          Address = new AddressData(model),
                                          LastName = model.LastName,
                                          Email = model.Email,
                                          Password = model.Password,
                                          Registered = DateTime.Now
                                      };

                    _users.AddUser(newUser);

                    SendMailAsync(newUser);
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ModelState.AddModelError("Email", "Пользователь с таким e-mail уже существует");
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RegisterAsBookshell(CreateShellViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddModelStateErrors(ModelState.Keys.SelectMany(key => ModelState[key].Errors), true);
            }
            else
            {
                model.ClearErrors();
                var anyUser = _users.GetUserByEmail(model.Email);
                if (anyUser == null)
                {
                    var user = new ShellUser
                        {
                            Name = model.Name,
                            ShellAddressData = new ShellAddressData
                                {
                                    Country = model.Country,
                                    Formatted = model.FormattedAddress,
                                    Location = new Location(model.Lat, model.Lng),
                                    LocalPath = model.LocalPath,
                                    Route = model.Route,
                                    StreetNumber = model.StreetNumber
                                },
                            Created = DateTime.UtcNow,
                            Id = ObjectId.GenerateNewId().ToString(),
                        };

                    _shellUserService.Insert(user);


                    SendMailAsync(user);
                    return Json(model);
                }
                else
                {
                    ModelState.AddModelError("Email", "Пользователь с таким e-mail уже существует");
                }
            }
      
            return Json(model);
        }

        [HttpGet]
        public ActionResult RegisterAsBookshell()
        {
            return View(new CreateShellViewModel());
        }

        private void SendMailAsync(User newUser)
        {
            Task.Factory.StartNew(() =>
            {
                var mailer = new MailService();
                var welcomeEmail = mailer.SendWelcomeMessage(newUser);
                welcomeEmail.Deliver();
            });
        }

        private void SendMailAsync(ShellUser newUser)
        {
            Task.Factory.StartNew(() =>
            {
                var mailer = new MailService();
                var welcomeEmail = mailer.SendWelcomeMessage(newUser);
                welcomeEmail.Deliver();
            });
        }

        public static string GetIdForUser()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        public ActionResult LoginWithFacebook()
        {
            return ProcessFb(FacebookCallbackMode.AuthorizeWithFacebook);
        }

        public ActionResult UpdateFacebookFields()
        {
            return ProcessFb(FacebookCallbackMode.UpdateFacebookFields, Request.UrlReferrer.AbsolutePath);
        }

        private ActionResult ProcessFb(FacebookCallbackMode mode, string returnUrl = null)
        {
            var fbToken = Session[SessionKeys.FbAccessToken] as string;

            if (mode == FacebookCallbackMode.AuthorizeWithFacebook)
            {
                if (fbToken == null)
                {
                    return RunFacebookCallback(FacebookCallbackMode.AuthorizeWithFacebook);
                }

                return ProcessFacebook(FacebookCallbackMode.AuthorizeWithFacebook);
            }

            if (mode == FacebookCallbackMode.UpdateFacebookFields)
            {
                if (fbToken == null)
                {
                    return RunFacebookCallback(FacebookCallbackMode.UpdateFacebookFields, returnUrl);
                }

                return ProcessFacebook(FacebookCallbackMode.UpdateFacebookFields, returnUrl);
            }

            return null;
        }

        private ActionResult RunFacebookCallback(FacebookCallbackMode mode, string returnUrl = null)
        {
            var csrfToken = Guid.NewGuid().ToString();

            Session[SessionKeys.FbCsrfToken] = csrfToken;
            Session[SessionKeys.FbCallbackMode] = mode;
            Session[SessionKeys.FbReturnUrl] = returnUrl;

            var state = Convert.ToBase64String(Encoding.UTF8.GetBytes(_fb.SerializeJson(new { returnUrl = returnUrl, csrf = csrfToken, mode = mode })));
            const string scope = "user_about_me,email,publish_actions";
            var fbLoginUrl = _fb.GetLoginUrl(
                new
                {
                    client_id = _settings.FacebookAppId,
                    client_secret = _settings.FacebookSecretKey,
                    redirect_uri = FacebookCallbackUri,
                    response_type = "code",
                    scope = scope,
                    state = state,
                });
            return Redirect(fbLoginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code, string state)
        {
            var fbCallbackMode = (FacebookCallbackMode)Session[SessionKeys.FbCallbackMode];
            var fbReturnUrl = (string)Session[SessionKeys.FbReturnUrl];

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
            {
                throw new Exception(string.Format("Can't process facebook. No code or state, code: {0}, state: {1}", code,state));
                return RunFacebookCallback(fbCallbackMode, fbReturnUrl);
            }

            // first validate the csrf token
            dynamic decodedState;
            try
            {
                decodedState = _fb.DeserializeJson(Encoding.UTF8.GetString(Convert.FromBase64String(state)), null);
                var exepectedCsrfToken = Session[SessionKeys.FbCsrfToken] as string;

                // make the fb_csrf_token invalid
                Session[SessionKeys.FbCsrfToken] = null;
                Session[SessionKeys.FbCallbackMode] = null;
                Session[SessionKeys.FbReturnUrl] = null;

                if (!(decodedState is IDictionary<string, object>) || !decodedState.ContainsKey("csrf") || string.IsNullOrWhiteSpace(exepectedCsrfToken) || exepectedCsrfToken != decodedState.csrf)
                {
                    throw new Exception(string.Format("Can't process facebook. No decodedState or exepectedCsrfToken or exepectedCsrfToken != decodedState.csrf, decodedState: {0}, exepectedCsrfToken: {1}", decodedState, exepectedCsrfToken));
                    return RunFacebookCallback(fbCallbackMode, fbReturnUrl);
                }
            }
            catch (Exception exception)
            {
                // log exception
                throw new Exception(string.Format("Can't process facebook.  fbCallbackMode: {0}, fbReturnUrl: {1}", fbCallbackMode, fbReturnUrl), exception);
                return RunFacebookCallback(fbCallbackMode, fbReturnUrl);
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

                if (decodedState.ContainsKey("returnUrl") && decodedState.ContainsKey("mode"))
                {
                    return ProcessFacebook((FacebookCallbackMode)decodedState.mode, decodedState.returnUrl);
                }

                return ProcessFacebook(FacebookCallbackMode.AuthorizeWithFacebook);

                // return RunFacebookCallback(fbCallbackMode, fbReturnUrl);
            }
            catch(Exception exception)
            {
                // log exception
                throw new Exception(string.Format("Can't process facebook.  fbCallbackMode: {0}, fbReturnUrl: {1}", fbCallbackMode, fbReturnUrl), exception);
                return RunFacebookCallback(fbCallbackMode, fbReturnUrl);
            }
        }

        private ActionResult ProcessFacebook(FacebookCallbackMode mode, string returnUrl = null)
        {
            _fb.AccessToken = Session[SessionKeys.FbAccessToken] as string;
            var fbUser = _fb.Get<FbUserMe>("me");
            var facebookId = fbUser.id;

            if (mode == FacebookCallbackMode.AuthorizeWithFacebook)
            {
                var userByFacebookId = _users.GetByFacebookId(facebookId);
                if (userByFacebookId == null)
                {
                    var address = new AddressData(fbUser.location.name);
                    var newUser = new User
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Email = fbUser.email,
                        FacebookId = fbUser.id,
                        FacebookAccessToken = Session[SessionKeys.FbAccessToken] as string,
                        FirstName = fbUser.first_name,
                        LastName = fbUser.last_name,
                        Address = address,
                        AvatarUrl = string.Format("https://graph.facebook.com/{0}/picture?width=250&height=250", fbUser.id),
                        Registered = DateTime.Now,
                    };

                    _users.Save(newUser);
                    Auth.LoginUser(newUser, true);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    userByFacebookId.FacebookAccessToken = Session[SessionKeys.FbAccessToken] as string;
                    _users.Save(userByFacebookId);
                    Auth.LoginUser(userByFacebookId, true);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            if (mode == FacebookCallbackMode.UpdateFacebookFields)
            {
                var userByFacebookId = _users.GetByFacebookId(facebookId);

                if (userByFacebookId == null || userByFacebookId.Id == UserId)
                {
                    var currentUser = _users.GetById(UserId);
                    currentUser.FacebookId = facebookId;
                    currentUser.FacebookAccessToken = Session[SessionKeys.FbAccessToken] as string;
                    _users.Save(currentUser);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                return View("UserWithFbIdAlreadyExist");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetFbFriends()
        {
            var currentUser = _users.GetById(UserId);

            if (currentUser.IsFacebookAccount)
            {
                try
                {
                    _fb.AccessToken = currentUser.FacebookAccessToken;
                    dynamic fbresult = _fb.Get("fql", new { q = "SELECT uid, first_name, last_name, pic_square FROM user WHERE uid in (SELECT uid2 FROM friend WHERE uid1 = me())" });
                    var fbfriendsInfo = fbresult["data"].ToString();
                    List<FacebookFriend> fbFriends = JsonConvert.DeserializeObject<List<FacebookFriend>>(fbfriendsInfo);

                    var fbIds = fbFriends.Select(x => x.FacebookId).ToList();

                    var existingUsersIds = _users.GetExistingUsersIds(fbIds).ToDictionary(x => x.FacebookId, c => c);
                    var existingFrends = fbFriends.Where(x => existingUsersIds.ContainsKey(x.FacebookId)).ToList();

                    foreach (var friend in existingFrends)
                    {
                        friend.Id = existingUsersIds[friend.FacebookId].Id;
                    }

                    var model = new FacebookSelectorViewModel(existingFrends);
                    return View(model);
                }
                catch (Exception ex)
                {
                    var returnUrl = Url.Action("GetFbFriends", "User");
                    return ProcessFb(FacebookCallbackMode.UpdateFacebookFields, returnUrl);
                }
            }

            return View();
        }
    }
}

