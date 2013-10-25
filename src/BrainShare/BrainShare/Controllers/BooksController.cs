﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrainShare.Documents;
using BrainShare.Extensions;
using BrainShare.GoogleDto;
using BrainShare.Hubs;
using BrainShare.Services;
using BrainShare.Services.Validation;
using BrainShare.ViewModels;
using MongoDB.Bson;

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
        private readonly CloudinaryImagesService _cloudinaryImages;

        public BooksController(UsersService users, BooksService books, ActivityFeedsService feeds, WishBooksService wishBooks, CloudinaryImagesService cloudinaryImages)
        {
            _users = users;
            _books = books;
            _feeds = feeds;
            _wishBooks = wishBooks;
            _cloudinaryImages = cloudinaryImages;
        }

        public ActionResult Index()
        {
            return View(new BooksFilterModel());
        }

        public ActionResult Filter(BooksFilterModel model)
        {
            var filter = model.ToFilter();
            var items = _books.GetByFilter(filter).Select(x=> new BookViewModel(x));
            model.UpdatePagingInfo(filter.PagingInfo);
            return Listing(items,model);
        }

        public ActionResult Search()
        {
            var model = new List<string>();
            if (UserId != null)
            {
                model = _books.GetUserBooks(UserId).Select(x => x.GoogleBookId).ToList();
            }
            return View(model);
        }

        [GET("info/{id}")]
        [GET("info/wish/{wishBookId}")]
        public ActionResult Info(string id, string wishBookId)
        {
            var book = id.HasValue() ? _books.GetById(id) : _wishBooks.GetById(wishBookId);
            if (book == null)
            {
                return View("NotFound");
            }
            var model = new BookViewModel(book);
            model.CurrentUserId = UserId;
            Title(model.Title);
            return View("Info", model);
        }

        [GET("edit/{id}")]
        [GET("edit/wish/{wishBookId}")]
        public ActionResult Edit(string id, string wishBookId)
        {
            var languages = new LanguagesService().GetAllLanguages();
            var book = id.HasValue() ? _books.GetById(id) : _wishBooks.GetById(wishBookId);
            var model = new EditBookViewModel(book, languages);
            model.IsWhishBook = wishBookId.HasValue();
            Title(model.Title);
            return View("Edit", model);
        }

        [POST("edit")]
        public ActionResult Edit(EditBookViewModel model)
        {
            AdditionalBookValidate(model);
            if (ModelState.IsValid)
            {
                var book = !model.IsWhishBook ? _books.GetById(model.Id) : _wishBooks.GetById(model.Id);
                model.UpdateBook(book);

                if (model.IsWhishBook)
                {
                    _wishBooks.Save(book);
                }
                else
                {
                    _books.Save(book);
                }
            }

            return JsonModel(model);
        }

        private void AdditionalBookValidate(EditBookViewModel model)
        {
            if (model.ISBNs.Count == 0)
            {
                ModelState.AddModelError("ISBNs", "Должен быть указан хотябы один ISBN");
            }
            if (model.ISBNs.Any(x => !x.Value.HasValue()))
            {
                ModelState.AddModelError("ISBNs", "ISBN не может быть пустым");
            }
            if (model.Authors.Count == 0)
            {
                ModelState.AddModelError("Authors", "Должен быть указан хотябы один автор");
            }
            if (model.Authors.Any(x => !x.Value.HasValue()))
            {
                ModelState.AddModelError("Authors", "Автор не может быть пустой строкой");
            }

            DateTime dt;
            var parsed = DateTime.TryParseExact(model.PublishedDate,
                                   EditBookViewModel.DateFormat,
                                   EditBookViewModel.Culture,
                                   DateTimeStyles.None,
                                   out dt);
            if (!parsed)
            {
                ModelState.AddModelError("PublishedDate", "У даты неправильный формат");
            }
        }

        [GET("choose-add-method")]
        public ActionResult ChooseAddMethod()
        {
            return View();
        }

        [GET("add")]
        public ActionResult Add()
        {
            var languages = new LanguagesService().GetAllLanguages();
            var model = new EditBookViewModel(languages);
            Title(model.Title);
            return View("Add", model);
        }

        [POST("add")]
        public ActionResult Add(EditBookViewModel model)
        {
            AdditionalBookValidate(model);
            if (ModelState.IsValid)
            {
                var book = new Book() { Id = model.Id ?? ObjectId.GenerateNewId().ToString() };
                model.UpdateBook(book);
                model.Id = book.Id;

                var user = _users.GetById(UserId);
                book.UserData = new UserData(user);
                if (model.IsWhishBook)
                {
                    _wishBooks.Save(book);
                }
                else
                {
                    _books.Save(book);
                }
            }

            return JsonModel(model);
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
            return Json(new { doc.Id });
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
                return Json(new { Error = "Книга уже добавлена." });
            }
            return Json(new { Id = doc.Id });
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
                Title(model.Book.Title);
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
            Title("Обмен " + model.Book.Title + " от " + user.FullName);
            return View(model);
        }

        [GET("accept/{requestedBookId}/from/{userId}")]
        public ActionResult AcceptRequestFrom(string requestedBookId, string userId)
        {
            var model = new AcceptRequestViewModel();
            model.AllBooks = _books.GetUserBooks(userId).Select(x => new BookViewModel(x)).ToList();
            model.BooksYouNeedTitles = _wishBooks.GetUserBooks(userId).Select(x => x.Title).ToList();
            var fromUser = _users.GetById(userId);
            model.FromUser = new UserItemViewModel(null, fromUser);
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
                return View("CustomError", (object)"Вы не можете обмениваться книгами с самим собой.");
            }
            try
            {
                var you = _users.GetById(UserId);
                var he = _users.GetById(userId);

                var himBook = _books.GetById(bookId);
                himBook.UserData = new UserData(you);
                _books.Save(himBook);
                _wishBooks.Delete(you.Id, himBook.GoogleBookId);

                var yourBook = _books.GetById(yourBookId);
                yourBook.UserData = new UserData(he);
                _books.Save(yourBook);
                _wishBooks.Delete(he.Id, yourBook.GoogleBookId);

                you.AddRecievedBook(bookId, userId);
                you.Inbox.RemoveAll(x => x.UserId == userId && x.BookId == yourBookId);
                _users.Save(you);

                he.AddRecievedBook(yourBookId, you.Id);
                _users.Save(he);

                SaveFeedAsync(ActivityFeed.BooksExchanged(yourBook, you, himBook, he));
                SendExchangeMail(yourBook, you, himBook, he);
                SendRequestAcceptedNotification(userId, yourBook, himBook, you);

                return View("ExchangeSucces", he);
            }
            catch
            {
                return View("CantExchangeError");
            }
        }

        private void SendRequestAcceptedNotification(string userId, Book book, Book onBook, User fromUser)
        {
            NotificationsHub.HubContext.Clients.Group(userId).requestAccepted(new RequestAcceptedModel(book, onBook, fromUser));
        }

        private void SendExchangeMail(Book yourBook, User you, Book hisBook, User he)
        {
            Task.Factory.StartNew(() =>
                {
                    var mailer = new MailService();
                    var emailTofirst = mailer.SendExchangeConfirmMessage(you, yourBook, he, hisBook);
                    emailTofirst.Deliver();
                    var mailer2 = new MailService();
                    var emailToSecond = mailer2.SendExchangeConfirmMessage(he, hisBook, you, yourBook);
                    emailToSecond.Deliver();
                });
        }

        private void SaveFeedAsync(ActivityFeed feed)
        {
            Task.Factory.StartNew(() => _feeds.Save(feed));
        }

        public JsonResult UploadBookImage(HttpPostedFileBase bookImgfile)
        {
            bool isValidImage;

            if (bookImgfile != null)
            {
                isValidImage = bookImgfile.IsValidImage(128, 180);
            }
            else
            {
                // TODO: actions if close button pressed
                return null;
            }

            if (isValidImage)
            {
                var cloudinary = new CloudinaryDotNet.Cloudinary(ConfigurationManager.AppSettings.Get("cloudinary_url"));
                bookImgfile.InputStream.Seek(0, SeekOrigin.Begin);
                var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
                {
                    File = new CloudinaryDotNet.Actions.FileDescription("filename", bookImgfile.InputStream),
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                string bookImgUrl = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(128).Height(180).Crop("fill")).BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));

                _cloudinaryImages.AddImage(new CloudinaryImage() { Id = ObjectId.GenerateNewId().ToString(), ImageUrl = bookImgUrl, ImageId = uploadResult.PublicId });
                return Json(new { bookImgUrl = bookImgUrl, bookImgId = uploadResult.PublicId });
            }
            return Json(new { error = "Файл загруженный вами не является изображением или его размеры слишком малы" });
        }
    }
}
