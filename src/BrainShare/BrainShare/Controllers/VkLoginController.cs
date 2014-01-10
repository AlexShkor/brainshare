using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using BrainShare.Infostructure;
using BrainShare.Services;
using BrainShare.Utils.Extensions;
using BrainShare.Utils.Utilities;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Facebook.Dto;
using Brainshare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Settings;
using Brainshare.Infrastructure.VK;
using Brainshare.VK;
using MongoDB.Bson;
using Newtonsoft.Json;
using Oauth.Vk.Api;
using Oauth.Vk.Dto.Geolocation;
using Oauth.Vk.Dto.VkUserApi;
using Oauth.Vk.Helpers;


namespace BrainShare.Controllers
{
    public class VkLoginController:BaseController
    {
        private readonly Settings _settings;
        private readonly CryptographicHelper _cryptographicHelper;
        private readonly IAuthentication _auth;

        const string Scope = "offline";

        public string VkCallbackUri
        {
            get
            {
                return UrlUtility.ApplicationBaseUrl + Url.Action("VkCallback");
            }
        }

        public VkLoginController(UsersService usersService, Settings settings,CryptographicHelper cryptographicHelper,IAuthentication authentication) : base(usersService)
        {
            _settings = settings;
            _cryptographicHelper = cryptographicHelper;
            _auth = authentication;
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
            var csrfToken = _cryptographicHelper.GetCsrfToken();

            Session[SessionKeys.VkCsrfToken] = csrfToken;
            Session[SessionKeys.VkCallbackMode] = mode;
            Session[SessionKeys.VkReturnUrl] = returnUrl;

            var vkLoginUrl = VkHelper.BuildAuthorizeUrl(_settings.VkAppId,Scope, VkCallbackUri,"code",csrfToken);

            return Redirect(vkLoginUrl);
        }


        public ActionResult VkCallback(string code, string state)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new Exception("Can't process vk. No code");
            }


            var exepectedCsrfToken = Session[SessionKeys.VkCsrfToken] as string;

            if (state == null || !state.Equals(exepectedCsrfToken))
            {
                throw new Exception("invalid csrf token");
            }

            // make the vk_csrf_token invalid
            Session[SessionKeys.VkCsrfToken] = null;
            Session[SessionKeys.VkCallbackMode] = null;
            Session[SessionKeys.VkReturnUrl] = null;

            var json = VkHelper.GetAccessToken(_settings.VkAppId, _settings.VkSecretKey, VkCallbackUri, code);
            var responce = JsonConvert.DeserializeObject<OAuthResponce>(json);

            Session[SessionKeys.VkUserId] = responce.UserId;

            Session[SessionKeys.VkAccessToken] = responce.AccessToken;
            if (responce.ExpiresIn != "0")
                Session[SessionKeys.VkExpiresIn] = DateTime.Now.AddSeconds(int.Parse(responce.ExpiresIn));

            //if (decodedState.ContainsKey("returnUrl") && decodedState.ContainsKey("mode"))
            //{
            //    return ProcessFacebook((FacebookCallbackMode)decodedState.mode, decodedState.returnUrl);
            //}

            return ProcessVkontakte(VkCallbackMode.AuthorizeWithVk);
        }

        private ActionResult ProcessVkontakte(VkCallbackMode mode, string returnUrl = null)
        {
            var accessToken = Session[SessionKeys.VkAccessToken] as string;
            var vkId = Session[SessionKeys.VkUserId] as string;

            var vkUserApi = new VkUserApi(accessToken);
            var vkGeoApi = new VkGeolocationApi(accessToken);

            var dto = vkUserApi.Users_Get<List<VkUser>>(new[] { "city", "country", "photo_200" }, new[] { vkId });
            var vkUser = dto.First();
            if (mode == VkCallbackMode.AuthorizeWithVk)
            {
                var userByVkId = _users.GetUserByLoginServiceInfo(LoginServiceTypeEnum.Vk, vkId);
                if (userByVkId == null)
                {
                    var country = vkUser.Country.HasValue() ? vkGeoApi.Places_GetCountryById<List<VkCountry>>(vkUser.Country).First().Name : "Belarus";
                    var city = vkUser.City.HasValue() ? vkGeoApi.Places_GetCityById<List<VkCity>>(vkUser.City).First().Name : "Minsk";

                    var address = new AddressData(country, city);

                    var newUser = new User
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        FacebookAccessToken = Session[SessionKeys.VkAccessToken] as string,
                        FirstName = vkUser.FirstName,
                        LastName = vkUser.LastName,
                        Registered = DateTime.Now,
                        AvatarUrl = vkUser.AvatarUrl,
                        Address = address
                    };


                    newUser.LoginServices.Add(new LoginService
                        {
                            LoginType = LoginServiceTypeEnum.Vk,
                            AccessToken = accessToken,
                            ServiceUserId = vkUser.UserId,
                            UseForNotifications = false
                        });

                    _users.Save(newUser);

                    _auth.LoginUser(LoginServiceTypeEnum.Vk,vkUser.UserId, true);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    var loginService = userByVkId.LoginServices.Single(l => l.LoginType == LoginServiceTypeEnum.Vk && l.ServiceUserId == vkUser.UserId);
                    loginService.AccessToken = Session[SessionKeys.VkAccessToken] as string;

                    _users.Save(userByVkId);
                    _auth.LoginUser(LoginServiceTypeEnum.Vk, vkUser.UserId, true);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            if (mode == VkCallbackMode.UpdateVkFields)
            {
                var user = _users.GetUserByLoginServiceInfo(LoginServiceTypeEnum.Vk, vkUser.UserId);

                if (user == null || user.Id == UserId)
                {
                    var currentUser = _users.GetById(UserId);
                    var loginService = currentUser.LoginServices.Single(l => l.LoginType == LoginServiceTypeEnum.Vk);

                    loginService.ServiceUserId = vkUser.UserId;
                    loginService.AccessToken = Session[SessionKeys.VkAccessToken] as string;

                    _users.Save(currentUser);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                return View("UserWithVkIdAlreadyExist");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}