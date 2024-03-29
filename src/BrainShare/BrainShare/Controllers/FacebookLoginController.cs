﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using BrainShare.Facebook;
using BrainShare.Services;
using BrainShare.Utils.Utilities;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Facebook;
using Brainshare.Infrastructure.Facebook.Dto;
using Brainshare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Settings;
using Facebook;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BrainShare.Controllers
{
    public class FacebookLoginController : BaseController
    {
        private readonly Settings _settings;
        private readonly FacebookClient _fb;
        private readonly IAuthentication _auth;

        public string FacebookCallbackUri
        {
            get
            {
                return UrlUtility.ApplicationBaseUrl + Url.Action("FacebookCallback");
            }
        }


        public FacebookLoginController(UsersService usersService, FacebookClientFactory fbFactory, Settings settings, IAuthentication auth)
            : base(usersService)
        {
            _settings = settings;
            _auth = auth;
            _fb = fbFactory.GetClient();
        }

        public ActionResult LoginWithFacebook()
        {
            return ProcessFb(FacebookCallbackMode.AuthorizeWithFacebook);
        }

        [Authorize]
        public ActionResult AddFbAccount()
        {
            return ProcessFb(FacebookCallbackMode.LinkNewAccount);
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

            if (mode == FacebookCallbackMode.LinkNewAccount)
            {
                return RunFacebookCallback(FacebookCallbackMode.LinkNewAccount, returnUrl);
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
                throw new Exception(string.Format("Can't process facebook. No code or state, code: {0}, state: {1}", code, state));
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
                }
            }
            catch (Exception exception)
            {
                // log exception
                throw new Exception(string.Format("Can't process facebook.  fbCallbackMode: {0}, fbReturnUrl: {1}", fbCallbackMode, fbReturnUrl), exception);
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

                return ProcessFacebook(fbCallbackMode, decodedState.returnUrl);

            }
            catch (Exception exception)
            {
                // log exception
                throw new Exception(string.Format("Can't process facebook.  fbCallbackMode: {0}, fbReturnUrl: {1}", fbCallbackMode, fbReturnUrl), exception);
            }
        }

        private ActionResult ProcessFacebook(FacebookCallbackMode mode, string returnUrl = null)
        {
            _fb.AccessToken = Session[SessionKeys.FbAccessToken] as string;
            var fbUser = _fb.Get<FbUserMe>("me");
            var facebookId = fbUser.id;

            if (mode == FacebookCallbackMode.AuthorizeWithFacebook)
            {
                var userByFacebookId = _users.GetUserByFbId(facebookId);
                if (userByFacebookId == null)
                {
                    var address = new AddressData(fbUser.location.name);
                    var settings = new UserSettings
                    {
                        NotificationSettings = new NotificationSettings
                        {
                            DuplicateMessagesToEmail = true,
                            NotifyByEmailIfAnybodyAddedMyWishBook = true
                        },
                    };
                    var newUser = new User
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        FirstName = fbUser.first_name,
                        LastName = fbUser.last_name,
                        Address = address,
                        AvatarUrl = string.Format("https://graph.facebook.com/{0}/picture?width=200&height=200", fbUser.id),
                        Registered = DateTime.Now,
                        FacebookId = fbUser.id,
                        FacebookAccessToken = Session[SessionKeys.FbAccessToken] as string,
                        Email = fbUser.email,
                        Settings = settings
                    };

                    _users.Save(newUser);
                    _auth.LoginFb(fbUser.id);

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
                    _auth.LoginFb(fbUser.id);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            if (mode == FacebookCallbackMode.UpdateFacebookFields)
            {
                UpdateFbFields(fbUser.id);
            }

            if (mode == FacebookCallbackMode.LinkNewAccount)
            {
                var user = _users.GetUserByFbId(fbUser.id);

                if (user == null)
                {
                    var currentUser = _users.GetById(UserId);

                    currentUser.FacebookAccessToken = Session[SessionKeys.FbAccessToken] as string;
                    currentUser.FacebookId = fbUser.id;

                    _users.Save(currentUser);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Accounts", "Profile");
                }

                UpdateFbFields(fbUser.id);
            }

            return RedirectToAction("Index", "Home");
        }

        private void UpdateFbFields(string facebookId)
        {
            var userByFacebookId = _users.GetUserByFbId(facebookId);

            if (userByFacebookId != null )
            {
                var currentUser = _users.GetById(UserId);

                currentUser.FacebookAccessToken = Session[SessionKeys.FbAccessToken] as string;

                _users.Save(currentUser);
            }
        }

        public ActionResult GetFbFriends()
        {
            Title("Друзья с фейсбука");
            var currentUser = _users.GetById(UserId);

            if (currentUser.IsFacebookAccount)
            {
                try
                {
                    _fb.AccessToken = currentUser.FacebookAccessToken;
                    dynamic fbresult = _fb.Get("fql",
                                               new
                                                   {
                                                       q =
                                                   "SELECT uid, first_name, last_name, pic_square FROM user WHERE uid in (SELECT uid2 FROM friend WHERE uid1 = me())"
                                                   });
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
