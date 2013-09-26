using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using BrainShare.Services;
using CloudinaryDotNet;

namespace BrainShare.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ActivityFeedsService _activityFeeds;
        private readonly UsersService _users;

        public HomeController(ActivityFeedsService activityFeeds, UsersService users)
        {
            _activityFeeds = activityFeeds;
            _users = users;
        }

        public ActionResult Index()
        {
            Title("Активность");
            var feeds = _activityFeeds.GetLast100();
            return View(feeds);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }
    }
}
