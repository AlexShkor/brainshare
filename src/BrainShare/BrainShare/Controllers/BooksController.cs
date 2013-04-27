using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Services;
using Newtonsoft.Json;

namespace BrainShare.Controllers
{
    public class BooksController : Controller
    {
        private readonly UsersService _users;
        private readonly BooksService _books;

        public string UserId {get { return ((UserIdentity) User.Identity).User.Id; }}

        public BooksController(UsersService users, BooksService books)
        {
            _users = users;
            _books = books;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Search()
        {
            var user = _users.GetById(UserId);
            return View(user.Books);
        }

        [HttpPost]
        public ActionResult Give(string book)
        {
            var doc = JsonConvert.DeserializeObject<Book>(book);
            var user = _users.GetById(UserId);
            if (user.Books.Contains(doc.Id))
            {
                return Json(new {Error = "This book already added;"});
            }
            user.Books.Add(doc.Id);
            _users.Save(user);
            var currentDoc = _books.GetById(doc.Id) ?? doc;
            currentDoc.Owners.Add(UserId);
            _books.Save(currentDoc);
            return Json(new { doc.Id });
        }

        [HttpPost]
        public ActionResult Take(string book)
        {
            var doc = JsonConvert.DeserializeObject<Book>(book);
            var user = _users.GetById(UserId);
            if (!user.WishList.Contains(doc.Id))
            {
                user.Books.Add(doc.Id);
                _users.Save(user);
            }
            var currentDoc = _books.GetById(doc.Id) ?? doc;
            currentDoc.Lookers.Add(UserId);
            _books.Save(currentDoc);
            return Json(new { doc.Id });
        }

        [HttpGet]
        [ActionName("Take")]
        public ActionResult TakeOne(string id)
        {
            var model = new TakeBookViewModel();
            var book = _books.GetById(id);
            if (book != null)
            {
                model.Book = new BookViewModel(book);
            }
            var owners = _users.GetOwners(id);
            model.Owners = owners.Select(x => new OwnerViewModel(x)).ToList();
            throw new NotImplementedException();
        }
    }

    public class TakeBookViewModel
    {
        public string Id { get; set; }

        public BookViewModel Book { get; set; }

        public List<OwnerViewModel> Owners { get; set; }

        public TakeBookViewModel()
        {
            Owners = new List<OwnerViewModel>();
        }
    }

    public class BookViewModel
    {
        public string Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string SearchInfo { get; set; }
        public int PageCount { get; set; }
        public string PublishedDate { get; set; }
        public string Publisher { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }

        public BookViewModel(Book book)
        {
            Id = book.Id;
            ISBN = book.ISBN;
            Title = book.Title;
            SearchInfo = book.SearchInfo;
            PageCount = book.PageCount;
            PublishedDate = book.PublishedDate;
            Publisher = book.Publisher;
            Subtitle = book.Subtitle;
            Image = book.Image;
        }
    }

    public class OwnerViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public OwnerViewModel(User doc)
        {
            UserId = doc.Id;
            UserName = string.Format("{0} {1}", doc.FirstName, doc.LastName);
        }
    }
}
