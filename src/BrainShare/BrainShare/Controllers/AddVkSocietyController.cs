using System;
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

        public AddVkSocietyController(Settings settings, UsersService usersService) : base(usersService)
        {
            _settings = settings;
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

        //public ActionResult TestPost()
        //{
        //    var result = _vkWallApi.Wall_Post<VkPost>("-65777060", "message", null, StatusName.FromGroup,
        //                                              GroupPostSign.Sign);
        //    return Json(result);
        //}

        public ActionResult Step2(string blankPageUrl)
        {
            var token = ExtractToken(blankPageUrl);
            var user = _users.GetById(UserId);
            user.VkMobileAccessToken = token;
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
