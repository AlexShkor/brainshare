using System.Web.Mvc;
using Brainshare.Infrastructure.Services;

namespace BrainShare.Controllers
{
    public class HelpController : BaseController
    {
        public HelpController(UsersService userService):base(userService)
        {

        }

        public ActionResult Index()
        {
            Title("Помощь");
            return View();
        }

        public ActionResult HowToAddBook()
        {
            Title("Как добавить книгу в коллекцию?");
            return View();
        }

        public ActionResult HowToSearchBook()
        {
            Title("Как найти нужную вам книгу?");
            return View();
        }

        public ActionResult HowToExchangeBook()
        {
            Title("Как обменяться книгами?");
            return View();
        }
    }
}
