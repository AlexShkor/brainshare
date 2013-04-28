using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Services;

namespace BrainShare.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly BooksService _books;
        private readonly UsersService _users;

        public ProfileController(BooksService books, UsersService users)
        {
            _books = books;
            _users = users;
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

        public ActionResult Inbox()
        {
            var user = _users.GetById(UserId);
            var books = _books.GetByIds(user.Inbox.Select(x => x.BookId));
            var model = new InboxViewModel();
            return View(model);
        }
    }

    public class InboxViewModel
    {
        public List<InboxItem> Items { get; set; }
    }

    public class InboxItem
    {
        public BookViewModel Book { get; set; }
        public UserItemViewModel User { get; set; }
    }

    public class UserItemViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public UserItemViewModel(User doc)
        {
            UserId = doc.Id;
            UserName = string.Format("{0} {1}", doc.FirstName, doc.LastName);
        }
    }

    public class BaseController : Controller
    {
        public string UserId
        {
            get
            {
                return  (((UserIdentity) User.Identity).User ?? new User()).Id;
            }
        }

    }
}
