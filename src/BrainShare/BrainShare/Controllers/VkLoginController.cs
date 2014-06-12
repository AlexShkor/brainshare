using System;
using System.Linq;
using System.Web.Mvc;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using BrainShare.Facebook;
using BrainShare.Infostructure;
using BrainShare.Services;
using BrainShare.Utils.Extensions;
using BrainShare.Utils.Utilities;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Settings;
using Brainshare.VK;
using Brainshare.Vk.Api;
using Brainshare.Vk.Helpers;
using Brainshare.Vk.Infrastructure;
using MongoDB.Bson;
using Newtonsoft.Json;
using StructureMap;


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
            return ProcessVk(VkCallbackMode.AuthorizeWithVk);
        }

        [Authorize]
        public ActionResult AddVkAccount()
        {
            return ProcessVk(VkCallbackMode.LinkNewAccount);
        }

        private ActionResult ProcessVk(VkCallbackMode mode)
        {
            return RunVkCallback(mode);
        }


        private ActionResult RunVkCallback(VkCallbackMode mode)
        {
            Session[SessionKeys.VkCallbackMode] = mode;
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
            var response = JsonConvert.DeserializeObject<OAuthResponce>(json);

            if (response.ExpiresIn != "0")
                Session[SessionKeys.VkExpiresIn] = DateTime.Now.AddSeconds(int.Parse(response.ExpiresIn));

            var mode = (VkCallbackMode) Session[SessionKeys.VkCallbackMode];
            if (mode == VkCallbackMode.AuthorizeWithVk)
            {
                return ProcessVkontakte(response.UserId, response.AccessToken);
            }
            return LinkNewAccount(response.UserId, response.AccessToken);
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

        private ActionResult LinkNewAccount(string vkId, string accessToken)
        {
            var vkuser = _users.GetUserByVkId(vkId);
            if (vkuser != null)
            {
                throw new InvalidOperationException("Такой пользователь Вконтакте уже зарегистрирован на сервисе");
            }
            var user = _users.GetById(UserId);
            user.VkId = vkId;
            user.VkAccessToken = accessToken;
            _users.Save(user);
            return RedirectToAction("Accounts", "Profile");
        }
    }
}