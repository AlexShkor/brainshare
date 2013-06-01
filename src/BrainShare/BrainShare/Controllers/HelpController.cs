using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrainShare.Controllers
{
    public class HelpController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HowToAddBook()
        {
            Subtitle("Как добавить книгу в коллекцию?");
            return View();
        }

        public ActionResult HowToSearchBook()
        {
            Subtitle("Как найти нужную вам книгу?");
            return View();
        }

        public ActionResult HowToExchangeBook()
        {
            Subtitle("Как обменяться книгами?");
            return View();
        }
    }
}
