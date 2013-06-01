using System.Web.Mvc;
using BrainShare.Services;

namespace BrainShare.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ActivityFeedsService _activityFeeds;

        public HomeController(ActivityFeedsService activityFeeds)
        {
            _activityFeeds = activityFeeds;
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
