using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Services;

namespace BrainShare.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly BooksService _books;

        public ProfileController(BooksService books)
        {
            _books = books;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyBooks()
        {
            var books = _books.GetUserBooks(UserId);
            var model = books.Select(x => new BookViewModel(x)).ToList();
            return View(model);
        }
    }

    public class BaseController: Controller
    {
        public string UserId { get { return ((UserIdentity)User.Identity).User.Id; } }

    }
}
