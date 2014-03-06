using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using BrainShare.Documents;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using BrainShare.Domain.Enums;
using BrainShare.GoogleDto;
using BrainShare.Infostructure;
using BrainShare.Services;
using BrainShare.Utils.Extensions;
using BrainShare.Utils.Utilities;
using BrainShare.ViewModels;
using Brainshare.Infrastructure.Hubs;
using Brainshare.Infrastructure.Services;
using Brainshare.Infrastructure.Services.Validation;
using Brainshare.Infrastructure.Settings;
using Brainshare.Infrastructure.Sitemap;
using MongoDB.Bson;

namespace BrainShare.Controllers
{
    [AttributeRouting.RoutePrefix("books")]
    [Authorize]
    public class BooksController : BaseController
    {
        private readonly BooksService _books;
        private readonly WishBooksService _wishBooks;
        private readonly CloudinaryImagesService _cloudinaryImages;
        private readonly AsyncTaskScheduler _asyncTaskScheduler;
        private readonly ExchangeHistoryService _exchangeHistory;
        private readonly MailService _mailService;
        private readonly OzIsbnService _ozIsbnService;
        private readonly Settings _settings;

        public BooksController(UsersService users, BooksService books, WishBooksService wishBooks, CloudinaryImagesService cloudinaryImages, 
            ExchangeHistoryService exchangeHistory, Settings settings, MailService mailService, AsyncTaskScheduler asyncTaskScheduler,
            OzIsbnService ozIsbnService):base(users)
        {
            _books = books;
            _wishBooks = wishBooks;
            _cloudinaryImages = cloudinaryImages;
            _exchangeHistory = exchangeHistory;
            _settings = settings;
            _mailService = mailService;
            _ozIsbnService = ozIsbnService;
            _asyncTaskScheduler = asyncTaskScheduler;
        }
        
        [AllowAnonymous]
        [GET("index/{search?}")]
        public ActionResult Index(string search,string ISBN)
        {
            Title("Книги на Brainshare");

            return View(new BooksFilterModel()
            {
                Languages = new LanguagesService().GetAllLanguages(),
                Search = search,
                ISBN = ISBN
            });
        }

        [AllowAnonymous]
        public ActionResult Filter(BooksFilterModel model)
        {
            var filter = model.ToFilter();
            var items = _books.GetByFilter(filter).Select(x => new BookViewModel(x));
            model.UpdatePagingInfo(filter.PagingInfo);
            return Listing(items, model);
        }

        [AllowAnonymous]
        [Sitemap(Frequency = SitemapFrequency.Hourly, Priority = 1.0)]
        public ActionResult Search()
        {
            var model = new SearchByGoogleViewModel();

            if (UserId != null)
            {
                model.MyBooksIds = _books.GetUserBooks(UserId).Select(x => x.GoogleBookId).ToList();
                model.WishBooksIds = _wishBooks.GetUserBooks(UserId).Select(x => x.GoogleBookId).ToList();
            }

            Title("Искать на Google Books");
            return View(model);
        }

        [AllowAnonymous]
        [Sitemap(Frequency = SitemapFrequency.Hourly, Priority = 1.0)]
        public ActionResult SearchOzBy()
        {
            Response.AddHeader("Access-Control-Allow-Origin", "http://oz.by/");
            Response.AddHeader("Access-Control-Allow-Methods", "GET");
            Response.AddHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");

            var wishBooksIds = _wishBooks.GetOzIds(UserId);
            var myBooksIds = _books.GetOzIds(UserId);

            var model = new SearchByOzViewModel
                {
                    MyBooksIds = myBooksIds,
                    WishBooksIds = wishBooksIds
                };
           

            Title("Искать на Oz");
            return View(model);
        }

        [AllowAnonymous]
        public async Task<string> DownloadString(string q, string page, int searchLimit)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.GetEncoding("KOI8-R");
                var url = "http://oz.by/search/?catalog_id=1101523&q=" + HttpUtility.UrlEncode(q, client.Encoding) +
                          "&page=" + page + "&search_limit="+searchLimit.ToString();
                var result = await client.DownloadStringTaskAsync(url);
                return result;
            }
        }

        [GET("info/{id}")]
        [GET("info/wish/{wishBookId}")]
        [AllowAnonymous]
        public ActionResult Info(string id, string wishBookId)
        {
            var book = id.HasValue() ? _books.GetById(id) : _wishBooks.GetById(wishBookId);
            if (book == null)
            {
                Title("Книга не найдена");
                return View("NotFound");
            }
            var model = new BookViewModel(book);
            model.CurrentUserId = UserId;
            Title(model.Title);
            if (id.HasValue())
            {
                return View("Info", model);
            }
            return View("WishInfo", model);
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
            if (model.Authors.Count == 0)
            {
                ModelState.AddModelError("Authors", "Должен быть указан хотябы один автор");
            }
            if (model.Authors.Any(x => !x.Value.HasValue()))
            {
                ModelState.AddModelError("Authors", "Автор не может быть пустой строкой");
            }

            if (model.PublishedDate.HasValue())
            {
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
        }

        [GET("choose-add-method")]
        [AllowAnonymous]
        public ActionResult ChooseAddMethod()
        {
            Title("Выбор метода поиска");
            return View();
        }

        [GET("add")]
        [AllowAnonymous]
        public ActionResult Add()
        {
            var languages = new LanguagesService().GetAllLanguages();
            var model = new EditBookViewModel(languages);
            model.Language = "ru";
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
                SendBookAddedEvent(user, book, model.IsWhishBook);
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
        [POST("my/add/from-google")]
        [POST("give")]
        public async Task<ActionResult> AddToMyBooks(GoogleBookDto bookDto)
        {
            var doc = _books.GetUserBook(bookDto.GoogleBookId, UserId);
            if (doc == null)
            {
                var user = _users.GetById(UserId);
                doc = bookDto.BuildDocument(user);

                _books.Save(doc);

                SendBookAddedEvent(user,doc);

            }
            else
            {
                return Json(new { Error = "Книга уже добавлена." });
            }
            return Json(new { Id = doc.Id });
        }

        private void SendBookAddedEvent(User user, Book doc, bool isWhishBook = false)
        {
            _asyncTaskScheduler.WallPostVkGroup(UrlUtility.GetBookLink(doc.Id, UrlUtility.ApplicationBaseUrl), string.Format("Новая книга\r\n\"{0}\"\r\n{1}", doc.Title, doc.UserData.Address.Locality));

            _asyncTaskScheduler.StartEmailSendSearchingUsersTask(user, doc, UrlUtility.ApplicationBaseUrl);

            _asyncTaskScheduler.StartSaveFeedTask(ActivityFeed.BookAdded(doc.Id, doc.Title, user.Id, user.FullName));

            _asyncTaskScheduler.UserHaveNewBookNotifyer(user, doc);

            NotificationsHub.SendGenericText(UserId, "Книга добавлена", string.Format("{0} добавлена в вашу книную полку", doc.Title));
        }

        [HttpPost]
        [ValidateInput(false)]
        [POST("wish/add/from-google")]
        [POST("take")]
        public ActionResult AddToWishBooks(GoogleBookDto bookDto)
        {
            var doc = _wishBooks.GetUserBook(bookDto.GoogleBookId, UserId);
            if (doc == null)
            {
                var user = _users.GetById(UserId);

                doc = bookDto.BuildDocument(user);
                _wishBooks.Save(doc);

                _asyncTaskScheduler.StartSaveFeedTask(ActivityFeed.BookWanted(doc.Id, doc.Title, user.Id, user.FullName));

                NotificationsHub.SendGenericText(UserId,"Запрос отправлен","Запрос на книгу " + doc.Title + " успешно отправлен");
            }
            return Json(new { doc.Id });
        }

        [HttpPost]
        [ValidateInput(false)]
        [POST("my/add/from-oz")]
        public async Task<ActionResult> AddToMyBooksFromOz(OzBookDto bookDto)
        {
            var user = _users.GetById(UserId);
            var doc = bookDto.BuildDocument(user);

            _books.Save(doc);

            SendBookAddedEvent(user, doc);

            return Json(new { Id = doc.Id });
        }


        [HttpPost]
        [ValidateInput(false)]
        [POST("wish/add/from-oz")]
        public async Task<ActionResult> AddToWishBooksFromOz(OzBookDto bookDto)
        {
            var user = _users.GetById(UserId);
            var doc = bookDto.BuildDocument(user);

            _wishBooks.Save(doc);

            _ozIsbnService.AddItem(doc.OzBookId, true);

            _asyncTaskScheduler.StartSaveFeedTask(ActivityFeed.BookWanted(doc.Id, doc.Title, user.Id, user.FullName));

            _asyncTaskScheduler.UserSearchedForNewBookNotifyer(user, doc);

            NotificationsHub.SendGenericText(UserId, "Книга добавлена в поиск",
                string.Format("{0} добавлена в ваш список поиска", doc.Title));

            return Json(new { Id = doc.Id });
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
        public async Task<ActionResult> SendExchangeRequest(string bookId, string userId)
        {
            if (userId == UserId)
            {
                Title("Отказ в действие");
                return View("CustomError", (object)"Вы не можете отправить запрос самому себе.");
            }
            var user = _users.GetById(userId);

            var book = _books.GetById(bookId);
            if (!user.Inbox.Any(x => x.BookId == bookId && x.User.UserId == UserId))
            {
                var me = _users.GetById(UserId);
                user.Inbox.Add(new ChangeRequest
                    {
                        User = new UserData(me),
                        BookId = bookId,
                        BookTitle = book.Title
                    });
                _users.Save(user);

                if (user.EmailConfirmed)
                {
                    _mailService.SendRequestMessage(_users.GetById(UserId), user, book,UrlUtility.ApplicationBaseUrl);
                }
                
            }
            var model = new ChangeRequestSentModel(book);
            Title("Обмен " + model.Book.Title + " от " + user.FullName);
            return View("ResulsViews/SendExchangeRequest",model);
        }

        [GET("consider/{requestedBookId}/from/{userId}")]
        public ActionResult ConsiderRequestFrom(string requestedBookId, string userId)
        {
            var model = new ConsiderRequestViewModel();
            model.AllBooks = _books.GetUserBooks(userId).Select(x => new BookViewModel(x)).ToList();
            model.BooksYouNeedTitles = _wishBooks.GetUserBooks(userId).Select(x => x.Title).ToList();
            var fromUser = _users.GetById(userId);
            model.FromUser = new UserItemViewModel(fromUser);
            var yourBook = _books.GetById(requestedBookId);
            model.YourBook = new BookViewModel(yourBook);

            _asyncTaskScheduler.StartUpdateRequestViewed(UserId, requestedBookId, userId);

            Title(string.Format("Запрос от {0} на книгу {1}", fromUser.FullName, yourBook.Title));
            return View(model);
        }

        [POST("exchange")]
        public async Task<ActionResult> Exchange(string userId, string bookId, string yourBookId)
        {
            if (userId == UserId)
            {
                return JsonError("Вы не можете обмениваться книгами с самим собой.");
            }
            try
            {
                var you = _users.GetById(UserId);
                var he = _users.GetById(userId);

                if (!you.Inbox.Any(x => x.BookId == yourBookId && x.User.UserId == userId))
                {
                    return JsonError("Не найден соответствующий запрос на обмен.");
                }

                var hisBook = _books.GetById(bookId);
                if (hisBook.UserData.UserId != userId)
                {
                    return JsonError("Книга уже не пренадлежит указанному пользователю.");
                }
                hisBook.UserData = new UserData(you);

                var yourBook = _books.GetById(yourBookId);
                if (yourBook.UserData.UserId != UserId)
                {
                    return JsonError("Книга уже не пренадлежит указанному пользователю.");
                }
                yourBook.UserData = new UserData(he);

                you.RemoveInboxItem(userId, yourBookId);

                _users.Save(you);

                _books.Save(hisBook);
                _books.Save(yourBook);

                _exchangeHistory.SaveExchange(userId, new ExchangeEntry(he, hisBook),
                                                                   new ExchangeEntry(you, yourBook));

                _asyncTaskScheduler.StartSaveFeedTask(ActivityFeed.BooksExchanged(yourBook, you, hisBook, he));

                SendExchangeMail(yourBook, you, hisBook, he);
                SendRequestAcceptedNotification(userId, yourBook, hisBook, you);

                return Json(new
                {
                    Success = true
                });
            }
            catch
            {
                return Json(new
                {
                    Error = "По каким-то причинам вы не можете произвести обмен."
                });
            }
        }

        [POST("make-gift")]
        public async Task<ActionResult> MakeGift(string userId, string bookId)
        {
            if (userId == UserId)
            {
                return JsonError("Вы не можете обмениваться книгами с самим собой.");
            }
            try
            {
                var me = _users.GetById(UserId);

                var user = _users.GetById(userId);
                var book = _books.GetById(bookId);
                if (book.UserData.UserId != UserId)
                {
                    return JsonError("Книга уже не пренадлежит указанному пользователю.");
                }

                book.UserData = new UserData(user);

                me.RemoveInboxItem(userId, bookId);
                _users.Save(me);

                _books.Save(book);

                _exchangeHistory.SaveExchange(userId, new ExchangeEntry(me, book, ExchangeActionEnum.Give), new ExchangeEntry(user));

                _asyncTaskScheduler.StartSaveFeedTask(ActivityFeed.BooksGifted(me, book, user));

                _mailService.SendGiftExchangeMessage(book, me, user,UrlUtility.ApplicationBaseUrl);

                SendRequestAcceptedAsGiftNotification(userId, book, me);

                return JsonSuccess();
            }
            catch
            {
                return JsonError("По каким-то причинам вы не можете произвести обмен.");
            }
        }

        [GET("view-exchange-history")]
        public ActionResult ViewExchangeHistory()
        {
            var model = new ViewExchangeHistoryViewModel(_exchangeHistory.GetFor(UserId), UserId);
            Title("История обмена");
            return View(model);
        }

        private void SendRequestAcceptedNotification(string userId, Book book, Book onBook, User fromUser)
        {
            NotificationsHub.HubContext.Clients.Group(userId).requestAccepted(new RequestAcceptedModel(book, onBook, fromUser));
        }

        private void SendRequestAcceptedAsGiftNotification(string userId, Book book, User fromUser)
        {
            NotificationsHub.HubContext.Clients.Group(userId).requestAccepted(new RequestAcceptedModel(book, fromUser));
        }

        private void SendExchangeMail(Book yourBook, User you, Book hisBook, User he)
        {
            _mailService.SendExchangeConfirmMessage(you, yourBook, he, hisBook,UrlUtility.ApplicationBaseUrl);
            _mailService.SendExchangeConfirmMessage(he, hisBook, you, yourBook,UrlUtility.ApplicationBaseUrl);        
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
                var cloudinary = new CloudinaryDotNet.Cloudinary(_settings.CloudinaryUrl);
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
