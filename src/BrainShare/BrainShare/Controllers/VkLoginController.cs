using System;
using System.Web.Mvc;
using BrainShare.Services;
using BrainShare.Utils.Utilities;
using Brainshare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Settings;
using Brainshare.Infrastructure.VK;
using Brainshare.VK;


namespace BrainShare.Controllers
{
    public class VkLoginController:BaseController
    {
        private readonly Settings _settings;
        const string Scope = "offline";

        public string VkCallbackUri
        {
            get
            {
                return UrlUtility.ApplicationBaseUrl + Url.Action("VkCallback");
            }
        }

        public VkLoginController(UsersService usersService, Settings settings) : base(usersService)
        {
            _settings = settings;
        }

        public ActionResult LoginWithVk()
        {
            return ProcessVk(VkCallbackMode.AuthorizeWithVk);
        }

        private ActionResult ProcessVk(VkCallbackMode mode, string returnUrl = null)
        {
            var vkToken = Session[SessionKeys.VkAccessToken] as string;

            if (mode == VkCallbackMode.AuthorizeWithVk)
            {
                if (vkToken == null)
                {
                    return RunVkCallback(VkCallbackMode.AuthorizeWithVk, returnUrl);
                }

                return ProcessVkontakte(VkCallbackMode.AuthorizeWithVk);
            }

            if (mode == VkCallbackMode.UpdateVkFields)
            {
                if (vkToken == null)
                {
                    return RunVkCallback(VkCallbackMode.UpdateVkFields, returnUrl);
                }

                return ProcessVkontakte(VkCallbackMode.UpdateVkFields, returnUrl);
            }

            return null;
        }


        private ActionResult RunVkCallback(VkCallbackMode mode, string returnUrl = null)
        {
            var csrfToken = Guid.NewGuid().ToString();

            Session[SessionKeys.VkCsrfToken] = csrfToken;
            Session[SessionKeys.VkCallbackMode] = mode;
            Session[SessionKeys.VkReturnUrl] = returnUrl;

            var vkLoginUrl = VkHelper.BuildAuthorizeUrl(_settings.VkAppId,Scope, VkCallbackUri,"code");

            return Redirect(vkLoginUrl);
        }


        public ActionResult VkCallback(string code)
        {
   
            var vkCallbackMode = (VkCallbackMode)Session[SessionKeys.VkCallbackMode];
            var vkReturnUrl = (string)Session[SessionKeys.VkReturnUrl];

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new Exception("Can't process vk. No code");
            }

            return null;

            // first validate the csrf token
            //dynamic decodedState;
            //try
            //{
            //    decodedState = _fb.DeserializeJson(Encoding.UTF8.GetString(Convert.FromBase64String(state)), null);
            //    var exepectedCsrfToken = Session[SessionKeys.FbCsrfToken] as string;

            //    // make the fb_csrf_token invalid
            //    Session[SessionKeys.FbCsrfToken] = null;
            //    Session[SessionKeys.FbCallbackMode] = null;
            //    Session[SessionKeys.FbReturnUrl] = null;

            //    if (!(decodedState is IDictionary<string, object>) || !decodedState.ContainsKey("csrf") || string.IsNullOrWhiteSpace(exepectedCsrfToken) || exepectedCsrfToken != decodedState.csrf)
            //    {
            //        throw new Exception(string.Format("Can't process facebook. No decodedState or exepectedCsrfToken or exepectedCsrfToken != decodedState.csrf, decodedState: {0}, exepectedCsrfToken: {1}", decodedState, exepectedCsrfToken));
            //    }
            //}
            //catch (Exception exception)
            //{
            //    // log exception
            //    throw new Exception(string.Format("Can't process facebook.  fbCallbackMode: {0}, fbReturnUrl: {1}", fbCallbackMode, fbReturnUrl), exception);
            //}

            //try
            //{
            //    dynamic result = _fb.Post("oauth/access_token",
            //                              new
            //                              {
            //                                  client_id = _settings.FacebookAppId,
            //                                  client_secret = _settings.FacebookSecretKey,
            //                                  redirect_uri = FacebookCallbackUri,
            //                                  code = code
            //                              });

            //    Session[SessionKeys.FbAccessToken] = result.access_token;
            //    if (result.ContainsKey("expires"))
            //        Session[SessionKeys.FbExpiresIn] = DateTime.Now.AddSeconds(result.expires);

            //    if (decodedState.ContainsKey("returnUrl") && decodedState.ContainsKey("mode"))
            //    {
            //        return ProcessFacebook((FacebookCallbackMode)decodedState.mode, decodedState.returnUrl);
            //    }

            //    return ProcessFacebook(FacebookCallbackMode.AuthorizeWithFacebook);

            //    // return RunFacebookCallback(fbCallbackMode, fbReturnUrl);
            //}
            //catch (Exception exception)
            //{
            //    // log exception
            //    throw new Exception(string.Format("Can't process facebook.  fbCallbackMode: {0}, fbReturnUrl: {1}", fbCallbackMode, fbReturnUrl), exception);
            //    return RunFacebookCallback(fbCallbackMode, fbReturnUrl);
            //}
        }

        private ActionResult ProcessVkontakte(VkCallbackMode mode, string returnUrl = null)
        {
        //    _fb.AccessToken = Session[SessionKeys.FbAccessToken] as string;
        //    var fbUser = _fb.Get<FbUserMe>("me");
        //    var facebookId = fbUser.id;

        //    if (mode == VkCallbackMode.AuthorizeWithVk)
        //    {
        //        var userByFacebookId = _users.GetByFacebookId(facebookId);
        //        if (userByFacebookId == null)
        //        {
        //            var address = new AddressData(fbUser.location.name);
        //            var newUser = new User
        //            {
        //                Id = ObjectId.GenerateNewId().ToString(),
        //                Email = fbUser.email,
        //                FacebookId = fbUser.id,
        //                FacebookAccessToken = Session[SessionKeys.FbAccessToken] as string,
        //                FirstName = fbUser.first_name,
        //                LastName = fbUser.last_name,
        //                Address = address,
        //                AvatarUrl = string.Format("https://graph.facebook.com/{0}/picture?width=250&height=250", fbUser.id),
        //                Registered = DateTime.Now,
        //            };

        //            _users.Save(newUser);
        //            Auth.LoginUser(newUser.Id, true);

        //            if (Url.IsLocalUrl(returnUrl))
        //            {
        //                return Redirect(returnUrl);
        //            }

        //            return RedirectToAction("Index", "Home");
        //        }

        //        else
        //        {
        //            userByFacebookId.FacebookAccessToken = Session[SessionKeys.FbAccessToken] as string;
        //            _users.Save(userByFacebookId);
        //            Auth.LoginUser(userByFacebookId.Email, true);

        //            if (Url.IsLocalUrl(returnUrl))
        //            {
        //                return Redirect(returnUrl);
        //            }

        //            return RedirectToAction("Index", "Home");
        //        }
        //    }

        //    if (mode == VkCallbackMode.UpdateVkFields)
        //    {
        //        var userByFacebookId = _users.GetByFacebookId(facebookId);

        //        if (userByFacebookId == null || userByFacebookId.Id == UserId)
        //        {
        //            var currentUser = _users.GetById(UserId);
        //            currentUser.FacebookId = facebookId;
        //            currentUser.FacebookAccessToken = Session[SessionKeys.FbAccessToken] as string;
        //            _users.Save(currentUser);

        //            if (Url.IsLocalUrl(returnUrl))
        //            {
        //                return Redirect(returnUrl);
        //            }

        //            return RedirectToAction("Index", "Home");
        //        }

        //        return View("UserWithFbIdAlreadyExist");
        //    }

            return RedirectToAction("Index", "Home");
        }
    }
}