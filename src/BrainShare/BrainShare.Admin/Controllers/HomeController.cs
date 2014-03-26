using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Documents;
using Brainshare.Infrastructure.Services;
using BrainShare.Services;

namespace BrainShare.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly BooksService _booksService;
        private readonly UsersService _usersService;

        public HomeController(BooksService booksService, UsersService usersService)
        {
            _booksService = booksService;
            _usersService = usersService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateBooksUserData()
        {
            var books = _booksService.GetAll();
            foreach (var book in books)
            {
                var user = _usersService.GetById(book.UserData.UserId);
                if (user != null)
                {
                    book.UserData = new UserData(user);
                    _booksService.Save(book);
                }
            }
            return Content("success");
        }
    }
}