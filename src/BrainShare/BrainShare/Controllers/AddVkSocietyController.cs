using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Services;
using Brainshare.Infrastructure.Settings;
using Oauth.Vk.Helpers;

namespace BrainShare.Controllers
{
    [Authorize]
    public class AddVkSocietyController : BaseController
    {
        private readonly Settings _settings;
        private readonly UsersService _usersService;

        public AddVkSocietyController(Settings settings, UsersService usersService) : base(usersService)
        {
            _settings = settings;
            _usersService = usersService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Step1()
        {
            var vkLoginUrl = VkHelper.BuildAuthorizeUrlForMobile(_settings.VkAppId, VkHelper.MobileScope);

            return Redirect(vkLoginUrl);
        }

        public ActionResult Step2(string blankPageUrl)
        {
            var token = ExtractToken(blankPageUrl);
            var user = _usersService.GetById(UserId);
            user.VkAccessToken = token;
            _users.Save(user);
            return RedirectToAction("Index");
        }

        private string ExtractToken(string url)
        {
            var uri = new Uri(url.Replace("#", "?"));
            return HttpUtility.ParseQueryString(uri.Query).Get("access_token");
        }

    }
}
