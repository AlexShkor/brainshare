using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Services;
using Newtonsoft.Json;

namespace BrainShare.Controllers
{
    [RoutePrefix("books")]
    [Authorize]
    public class BooksController : BaseController
    {
        private readonly UsersService _users;
        private readonly BooksService _books;
        private readonly ActivityFeedsService _feeds;


        public BooksController(UsersService users, BooksService books, ActivityFeedsService feeds)
        {
            _users = users;
            _books = books;
            _feeds = feeds;
        }

        public ActionResult Search()
        {
            var model = new List<string>();
            if (UserId != null)
            {
                model = _users.GetById(UserId).Books.ToList();
            }
            return View(model);
        }

        [GET("info/{id}")]
        public ActionResult Info(string id)
        {
            var book = _books.GetById(id);
            var model = new BookViewModel(book);
            return View("Info", model);
        }


        [POST("info")]
        [ValidateInput(false)]
        public ActionResult InfoPost(Book doc)
        {
            var existing = _books.GetById(doc.Id);
            if (existing == null)
            {
                _books.Save(doc);
            }
            return Json(new { doc.Id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Give(Book doc)
        {
            var user = _users.GetById(UserId);
            if (user.Books.Contains(doc.Id))
            {
                return Json(new { Error = "This book already added;" });
            }
            user.Books.Add(doc.Id);
            _users.Save(user);
            var currentDoc = _books.GetById(doc.Id) ?? doc;
            currentDoc.Owners.Add(UserId);
            _books.Save(currentDoc);
            SaveFeedAsync(ActivityFeed.BookAdded(doc.Id, doc.Title, user.Id, user.FullName));
            return Json(new { doc.Id });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Take(Book doc)
        {
            var user = _users.GetById(UserId);
            if (!user.WishList.Contains(doc.Id))
            {
                user.WishList.Add(doc.Id);
                _users.Save(user);
                var currentDoc = _books.GetById(doc.Id) ?? doc;
                currentDoc.Lookers.Add(UserId);
                _books.Save(currentDoc);
                SaveFeedAsync(ActivityFeed.BookWanted(doc.Id, doc.Title, user.Id, user.FullName));
            }
            return Json(new { doc.Id });
        }

        [HttpGet]
        [ActionName("Take")]
        public ActionResult TakeOne(string id)
        {
            var model = new TakeBookViewModel();
            model.Id = id;
            var book = _books.GetById(id);
            if (book != null)
            {
                model.Book = new BookViewModel(book);
            }
            var owners = _users.GetOwners(id);
            model.Owners = owners.Where(x => x.Id != UserId).Select(x => new UserItemViewModel(x)).ToList();
            return View(model);
        }

        [GET("take/{bookId}/from/{userId}")]
        public ActionResult TakeFromUser(string bookId, string userId)
        {
            if (userId == UserId)
            {
                return View("CustomError", (object)"Вы не можете отправить запрос самому себе.");
            }
            var user = _users.GetById(userId);

            var book = _books.GetById(bookId);
            if (!user.Inbox.Any(x => x.BookId == bookId && x.UserId == UserId))
            {
                user.Inbox.Add(new ChangeRequest
                    {
                        UserId = UserId,
                        BookId = bookId
                    });
                _users.Save(user);
                var currentUser = _users.GetById(UserId);

                var mailer = new MailService();
                var requestEmail = mailer.SendRequestMessage(currentUser, user, book);
                requestEmail.Deliver();
            }
            var model = new ChangeRequestSentModel(book, user);
            return View(model);
        }

        [GET("accept/{requestedBookId}/from/{userId}")]
        public ActionResult AcceptRequestFrom(string requestedBookId, string userId)
        {
            var currentUser = _users.GetById(UserId);
            var books = _books.GetUserBooks(userId).ToList();
            var model = new AcceptRequestViewModel();
            model.AllBooks = books.Select(x => new BookViewModel(x)).ToList();
            model.BooksYouNeed = books.Where(x => currentUser.WishList.Contains(x.Id)).Select(x => new BookViewModel(x)).ToList();
            var fromUser = _users.GetById(userId);
            model.FromUser = new UserItemViewModel(fromUser);
            var yourBook = _books.GetById(requestedBookId);
            model.YourBook = new BookViewModel(yourBook);
            return View(model);
        }

        [GET("give/{yourBookId}/take/{bookId}/from/{userId}")]
        public ActionResult Exchange(string userId, string bookId, string yourBookId)
        {
            if (userId == UserId)
            {
                return View("CustomError", (object)"Вы не можете бмениваться книгами с самим собой.");
            }
            var you = _users.GetById(UserId);
            if (you.Books.Contains(yourBookId))
            {
                var him = _users.GetById(userId);
                if (him.Books.Contains(bookId) && him.WishList.Contains(yourBookId))
                {
                    you.Books.Remove(yourBookId);
                    you.WishList.Remove(bookId);
                    you.AddRecievedBook(bookId, userId);
                    you.Inbox.RemoveAll(x => x.UserId == userId && x.BookId == yourBookId);
                    _users.Save(you);


                    him.Books.Remove(bookId);
                    him.WishList.Remove(yourBookId);
                    him.AddRecievedBook(yourBookId, you.Id);
                    _users.Save(him);

                    var himBook = _books.GetById(bookId);
                    himBook.Lookers.Remove(you.Id);
                    himBook.Owners.Remove(him.Id);
                    _books.Save(himBook);


                    var yourBook = _books.GetById(yourBookId);
                    yourBook.Owners.Remove(you.Id);
                    yourBook.Lookers.Remove(him.Id);
                    _books.Save(yourBook);

                    SaveFeedAsync(ActivityFeed.BooksExchanged(yourBook, you, himBook, him));
                    SendExchangeMail(yourBook, you, himBook, him);

                    return RedirectToAction("MyBooks", "Profile");
                }
            }
            return View("CantExchangeError");
        }

        private void SendExchangeMail(Book yourBook, User you, Book hisBook, User he)
        {
            var mailer = new MailService();
            var emailTofirst = mailer.SendExchangeConfirmMessage(you, yourBook, he, hisBook);
            emailTofirst.Deliver();
            var mailer2 = new MailService();
            var emailToSecond = mailer2.SendExchangeConfirmMessage(he, hisBook, you, yourBook);
            emailToSecond.Deliver();
        }

        private void SaveFeedAsync(ActivityFeed feed)
        {
            Task.Factory.StartNew(() => _feeds.Save(feed));
        }
    }
}
