using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.GoogleDto;
using BrainShare.Hubs;
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
        private readonly WishBooksService _wishBooks;
        private readonly ActivityFeedsService _feeds;


        public BooksController(UsersService users, BooksService books, ActivityFeedsService feeds, WishBooksService wishBooks)
        {
            _users = users;
            _books = books;
            _feeds = feeds;
            _wishBooks = wishBooks;
        }

        public ActionResult Index()
        {
            var books = _books.GetPaged(0, 50);
            var model = new AllBooksViewModel(books);
            return View(model);
        }

        public ActionResult Search()
        {
            var model = new List<string>();
            if (UserId != null)
            {
                model = _books.GetUserBooks(UserId).Select(x=> x.GoogleBookId).ToList();
            }
            return View(model);
        }

        [GET("info/{id}")]
        [GET("info/wish/{wishBookId}")]
        public ActionResult Info(string id, string wishBookId)
        {
            var book = id.HasValue() ? _books.GetById(id) : _wishBooks.GetById(wishBookId);
            var model = new BookViewModel(book);
            return View("Info", model);
        }


        [POST("info")]
        [ValidateInput(false)]
        public ActionResult InfoPost(GoogleBookDto dto)
        {
            var doc = _books.GetByGoogleBookId(dto.GoogleBookId).FirstOrDefault();
            if (doc == null)
            {
                //no such books on the service
            }
            return Json(new {doc.Id});
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Give(GoogleBookDto bookDto)
        {
            var doc = _books.GetUserBook(bookDto.GoogleBookId, UserId);
            if (doc == null)
            {
                var user = _users.GetById(UserId);
                doc = bookDto.BuildDocument(user);
                SaveFeedAsync(ActivityFeed.BookAdded(doc.Id, doc.Title, user.Id, user.FullName));
                NotificationsHub.SendGenericText(UserId, "Книга добавлена",
                    string.Format("{0} добавлена в вашу книную полку", doc.Title));
                _books.Save(doc);
            }
            else
            {
                return Json(new {Error = "Книга уже добавлена."});
            }
            return Json(new {Id = doc.Id});
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Take(GoogleBookDto bookDto)
        {
            var doc = _wishBooks.GetUserBook(bookDto.GoogleBookId, UserId);
            if (doc == null)
            {
                var user = _users.GetById(UserId);
                doc = bookDto.BuildDocument(user);
                _wishBooks.Save(doc);
                SaveFeedAsync(ActivityFeed.BookWanted(doc.Id, doc.Title, user.Id, user.FullName));
            }
            return Json(new { doc.Id });
        }

        [HttpGet]
        [ActionName("Take")]
        public ActionResult SimilarTo(string id)
        {
            var model = new TakeBookViewModel();
            model.Id = id;
            var book = _wishBooks.GetById(id);
            if (book != null)
            {
                model.Book = new BookViewModel(book);
                model.Owners = _books.GetByGoogleBookId(book.GoogleBookId).Select(x => new UserItemViewModel(x)).ToList();
            }
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

                Task.Factory.StartNew(() =>
                {
                    var currentUser = _users.GetById(UserId);
                    var mailer = new MailService();
                    var requestEmail = mailer.SendRequestMessage(currentUser, user, book);
                    requestEmail.Deliver();
                });
            }
            var model = new ChangeRequestSentModel(book, user);
            return View(model);
        }

        [GET("accept/{requestedBookId}/from/{userId}")]
        public ActionResult AcceptRequestFrom(string requestedBookId, string userId)
        {
            var model = new AcceptRequestViewModel();
            model.AllBooks = _books.GetUserBooks(userId).Select(x => new BookViewModel(x)).ToList();
            model.BooksYouNeedTitles = _wishBooks.GetUserBooks(userId).Select(x => x.Title).ToList();
            var fromUser = _users.GetById(userId);
            model.FromUser = new UserItemViewModel(null,fromUser);
            var yourBook = _books.GetById(requestedBookId);
            model.YourBook = new BookViewModel(yourBook);
            UpdateRequestViewedAsync(UserId, requestedBookId, userId);
            Title(string.Format("Запрос от {0} на книгу {1}", fromUser.FullName, yourBook.Title));
            return View(model);
        }

        private void UpdateRequestViewedAsync(string userId, string bookId, string requestFromUserId)
        {
            Task.Factory.StartNew(() => _users.UpdateRequestViewed(userId, bookId, requestFromUserId));
        }

        [GET("give/{yourBookId}/take/{bookId}/from/{userId}")]
        public ActionResult Exchange(string userId, string bookId, string yourBookId)
        {
            if (userId == UserId)
            {
                return View("CustomError", (object) "Вы не можете бмениваться книгами с самим собой.");
            }
            try
            {
                var you = _users.GetById(UserId);
                var him = _users.GetById(userId);

                var himBook = _books.GetById(bookId);
                himBook.UserData = new UserData(you);
                _books.Save(himBook);
                _wishBooks.Delete(you.Id, himBook.GoogleBookId);

                var yourBook = _books.GetById(yourBookId);
                yourBook.UserData = new UserData(him);
                _books.Save(yourBook);
                _wishBooks.Delete(him.Id, yourBook.GoogleBookId);

                you.AddRecievedBook(bookId, userId);
                you.Inbox.RemoveAll(x => x.UserId == userId && x.BookId == yourBookId);
                _users.Save(you);

                him.AddRecievedBook(yourBookId, you.Id);
                _users.Save(him);

                SaveFeedAsync(ActivityFeed.BooksExchanged(yourBook, you, himBook, him));
                SendExchangeMail(yourBook, you, himBook, him);
                SendRequestAcceptedNotification(userId, yourBook, himBook, you);
            }
            catch
            {
                return View("CantExchangeError");
            }
            return RedirectToAction("MyBooks", "Profile");
        }

        private void SendRequestAcceptedNotification(string userId, Book book, Book onBook, User fromUser)
        {
            NotificationsHub.HubContext.Clients.Group(userId).requestAccepted(new RequestAcceptedModel(book, onBook, fromUser));
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

    public class RequestAcceptedModel
    {
        public string Message { get; set; }
        public string Title { get; set; }

        public RequestAcceptedModel(Book book, Book onBook, User fromUser)
        {
            Title = "Запрос на обмен принят";
            Message = string.Format("Ваз запрос на обмен книги {0} пользователя {2} на вашу книгу {1} был принят.",
                book.Title, fromUser.FullName, onBook.Title);
        }
    }

    public class AllBooksViewModel
    {
        public AllBooksViewModel(IEnumerable<Book> books)
        {
            Items = books.Select(x => new BookViewModel(x)).ToList();
        }

        public List<BookViewModel> Items { get; set; }
    }
}
