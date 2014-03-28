using System.Linq;
using System.Web.Mvc;
using BrainShare.ViewModels;
using Brainshare.Infrastructure.Services;

namespace BrainShare.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ActivityFeedsService _activityFeeds;
        private readonly BooksService _books;

        public HomeController(ActivityFeedsService activityFeeds, UsersService users, BooksService books):base(users)
        {
            _activityFeeds = activityFeeds;
            _books = books;
        }

        public ActionResult Index()
        {
            Title("Книги. Социальный портал по обмену книгами");
            var last5Users = _users.GetLast(5);
            var last5Books = _books.GetLast(5);
            var feeds = _activityFeeds.GetLast(10);
            var model = new HomeViewModel
            {
                Feeds = feeds,
                Users = last5Users.Select(x => new UserViewModel(x)),
                Books = last5Books.Select(x => new BookViewModel(x)),
            };
           return View("Main", model);
        }

        public ActionResult Activity()
        {
            Title("Активность");
            var feeds = _activityFeeds.GetLast100();
            return View(feeds);
        }

        public ActionResult About()
        {
            Title("О сервисе");
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Title("Контакты");
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Feedback()
        {
            Title("Отзывы");
            return IsShellUser ? View("ShellFeedback") : View();
        }
    }
}
