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
    }
}
