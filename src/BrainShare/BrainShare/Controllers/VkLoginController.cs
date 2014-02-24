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
using Brainshare.VK;
using Brainshare.Vk.Api;
using Brainshare.Vk.Helpers;
using Brainshare.Vk.Infrastructure;
using MongoDB.Bson;
using Newtonsoft.Json;


namespace BrainShare.Controllers
{
   // [Authorize]
    public class VkLoginController:BaseController
    {
        private readonly Settings _settings;
        private readonly CryptographicHelper _cryptographicHelper;
        private readonly IAuthentication _auth;

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

        [AllowAnonymous]
        public ActionResult LoginWithVk()
        {
            return ProcessVk();
        }

        [AllowAnonymous]
        public ActionResult AddVkAccount()
        {
            return ProcessVk();
        }

        private ActionResult ProcessVk()
        {
            return RunVkCallback();
        }


        private ActionResult RunVkCallback()
        {
            var vkLoginUrl = VkAuth.BuildAuthorizeUrl(_settings.VkAppId, VkCallbackUri);
            return Redirect(vkLoginUrl);
        }


        public ActionResult VkCallback(string code, string state)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new Exception("Can't process vk. No code");
            }

            var json = VkAuth.GetAccessToken(_settings.VkAppId, _settings.VkSecretKey, VkCallbackUri, code);
            var responce = JsonConvert.DeserializeObject<OAuthResponce>(json);

            if (responce.ExpiresIn != "0")
                Session[SessionKeys.VkExpiresIn] = DateTime.Now.AddSeconds(int.Parse(responce.ExpiresIn));

            return ProcessVkontakte(responce.UserId, responce.AccessToken);
        }

        private ActionResult ProcessVkontakte(string vkId, string accessToken)
        {
            var vkUserApi = new VkApi(accessToken);
            var vkGeoApi = new VkApi(accessToken);
            var dto = vkUserApi.GetUsers(new[] {"city", "country", "photo_200"}, new[] {vkId});
            var vkUser = dto.First();
            var user = _users.GetUserByVkId(vkId);
            if (user == null)
            {
                var country = vkUser.Country.HasValue()
                    ? vkGeoApi.GetCountry(vkUser.Country).Name
                    : "Belarus";
                var city = vkUser.City.HasValue()
                    ? vkGeoApi.GetCity(vkUser.City).Name
                    : "Minsk";

                var address = new AddressData(country, city);
                var settings = new UserSettings
                {
                    NotificationSettings = new NotificationSettings
                    {
                        DuplicateMessagesToEmail = true,
                        NotifyByEmailIfAnybodyAddedMyWishBook = true
                    },
                };
                user = new User
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    VkAccessToken = accessToken,
                    VkId = vkUser.UserId,
                    FirstName = vkUser.FirstName,
                    LastName = vkUser.LastName,
                    Registered = DateTime.Now,
                    AvatarUrl = vkUser.AvatarUrl,
                    Address = address,
                    Settings = settings
                };
            }
            else
            {
                user.VkAccessToken = accessToken;
          
            }
            _users.Save(user);
            _auth.LoginVk(user.VkId);

            return RedirectToAction("Index", "Home");
        }
    }
}