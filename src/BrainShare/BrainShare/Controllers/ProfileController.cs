using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using BrainShare.Hubs;
using BrainShare.Infostructure;
using BrainShare.Services;
using BrainShare.Utils.Extensions;
using BrainShare.Utils.Utilities;
using BrainShare.ViewModels;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Hubs;
using Brainshare.Infrastructure.Services;
using Brainshare.Infrastructure.Settings;
using MongoDB.Bson;


namespace BrainShare.Controllers
{
    [System.Web.Mvc.Authorize]
    [AttributeRouting.RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        private readonly BooksService _books;
        private readonly WishBooksService _wishBooks;
        private readonly ShellUserService _shellUserService;
        private readonly ThreadsService _threads;
        private readonly NewsService _news;
        private readonly CloudinaryImagesService _cloudinaryImages;
        private readonly MailService _mailService; 
        private readonly CryptographicHelper _cryptographicHelper;
        private readonly IAuthentication _authentication;
        private readonly AsyncTaskScheduler _asyncTaskScheduler;
        private readonly Settings _settings;

        public ProfileController(BooksService books, UsersService users, ShellUserService shellUserService, ThreadsService threads, WishBooksService whishBooks, 
            CloudinaryImagesService cloudinaryImages,CryptographicHelper cryptographicHelper,IAuthentication authentication, NewsService news, Settings settings,
            MailService mailService, AsyncTaskScheduler asyncTaskScheduler):base(users)
        {
            _books = books;
            _threads = threads;
            _wishBooks = whishBooks;
            _cloudinaryImages = cloudinaryImages;
            _shellUserService = shellUserService;
            _mailService = mailService;
            _asyncTaskScheduler = asyncTaskScheduler;
            _news = news;
            _cryptographicHelper = cryptographicHelper;
            _authentication = authentication;
            _settings = settings;
        }

        public ActionResult Index()
        {
            if (IsShellUser)
            {
                var shellUser = _shellUserService.GetById(UserId);
                Title(shellUser.Name);
                var model = new MyShellProfileViewModel(shellUser);

                return View("ShellProfileViewModel", model);
            }
            else
            {
                var user = _users.GetById(UserId);
                Title(user.FullName + " мой аккаунт");

                var model = new MyProfileViewModel(user);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            if (IsShellUser)
            {
                var shellUser = _shellUserService.GetById(UserId);
                var model = new EditShellProfileViewModel(shellUser);
                return View("EditShellProfile",model);

            }
            else
            {
                var user = _users.GetById(UserId);
                var model = new EditProfileViewModel(user);
                return View(model);
            }
     
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _users.GetById(UserId);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Info = model.Info;
                user.Address.Original = model.original_address;
                user.Address.Formatted = model.formatted_address;
                user.Address.Country = model.country;
                user.Address.Locality = model.locality;
                _users.Save(user);
            }

            model.Errors.RemoveAll(x => x.ErrorMessage != " ");
            return JsonModel(model);
        }

        [HttpPost]
        public ActionResult EditShellProfile(EditShellProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var shellUser = _shellUserService.GetById(UserId);

                shellUser.Name = model.Name;
                shellUser.ShellAddressData.LocalPath = model.LocalPath;
                shellUser.ShellAddressData.Formatted = model.FormattedAddress;
                shellUser.ShellAddressData.Country = model.Country;
                shellUser.ShellAddressData.StreetNumber = model.StreetNumber;
                shellUser.ShellAddressData.Route = model.Route;
                shellUser.ShellAddressData.Location = new Location(model.Lat,model.Lng);

                _shellUserService.Save(shellUser);
            }

            return JsonModel(model);
        }

        [GET("view/{id}")]
        [AllowAnonymous]
        public ActionResult ViewUserProfile(string id)
        {
            if (id == UserId)
            {
                return RedirectToAction("Index", "Profile");
            }

            var user = _users.GetById(id);
            var me = _users.GetById(UserId);
  
            var model = new UserProfileModel(user, me,_settings.ActivityTimeoutInMinutes);

            if (UserId.HasValue())
            {
                model.CanIncrease = user.GetVote(id, UserId) <= 0;
                model.CanDecrease = user.GetVote(id, UserId) >= 0;
                model.SummaryVotes = user.GetSummaryVotes();
            }

            Title(user.FullName);
            return View(model);
        }

        [POST]
        [ValidateInput(false)]
        public ActionResult DontHave(BookViewModel book)
        {
            try
            {
                _books.Remove(book.Id);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }

            return Json(new { Success = "OK" });
        }

        [POST]
        public ActionResult UpdateBookStatus(string id)
        {
            var book = _books.GetById(id);
            book.IsUserReadMe = !book.IsUserReadMe;

            _books.Save(book);

            return Json(book.IsUserReadMe);
        }
        

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordModel{ IsFacebokAccountWithoutPassword = IsFacebookAccountWithoutPassword});
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var salt = _cryptographicHelper.GenerateSalt();
                var password = _cryptographicHelper.GetPasswordHash(model.Password, salt);

                if (IsShellUser)
                {
                    var shellUser = _shellUserService.GetById(UserId);
                    shellUser.Password = password;
                    shellUser.Salt = salt;
                    _shellUserService.Save(shellUser);
                }
                else
                {
                    var user = _users.GetById(UserId);
                    user.Password = password;
                    user.Salt = salt;

                    _users.Save(user);
                }

                _authentication.Logout();
            }

            return JsonModel(model);
        }



        [POST]
        [ValidateInput(false)]
        public ActionResult DontWant(BookViewModel book)
        {
            try
            {
                _wishBooks.Remove(book.Id);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }

            return Json(new { Success = "OK" });
        }

        public ActionResult MyBooks()
        {
            Title("Мои Книги");
            var books = _books.GetUserBooks(UserId);
            var model = books.Select(x => new BookViewModel(x)).ToList();
            return View(model);
        }

        public ActionResult WishList()
        {
            Title("Я ищу Книги");
            var books = _wishBooks.GetUserBooks(UserId);
            var model = books.Select(x => new BookViewModel(x)).ToList();
            return View(model);
        }

        public ActionResult Inbox()
        {
            var z = UrlUtility.ApplicationBaseUrl;
            Title("Входящие");
            var user = _users.GetById(UserId);
            var model = new InboxViewModel();
            model.Items = user.Inbox.OrderByDescending(x => x.Created).Select(x =>
                                            new InboxItem(x)).ToList();
            return View(model);
        }

        [GET("reject/{bookId}/from/{userId}")]
        public ActionResult Reject(string bookId, string userId)
        {
            var user = _users.GetById(UserId);
            user.Inbox.RemoveAll(x => x.BookId == bookId && x.User.UserId == userId);
            _users.Save(user);
            return RedirectToAction("Inbox");
        }


        [GET("messages")]
        public ActionResult AllMessages()
        {
            if (IsShellUser)
            {
                return View("ShellAllMessages");
            }

            var threads = _threads.GetAllForUser(UserId).Where(x => x.Messages.Any()).OrderByDescending(x => x.Messages.Max(m => m.Posted));
            var user = _users.GetById(UserId);
            var model = new AllThreadsViewModel(threads, UserId, user);
            Title("Сообщения");

            return View(model);
        }

        [GET("settings")]
        public ActionResult Settings()
        {
            var user = _users.GetById(UserId);
            var seetings = user.Settings.NotificationSettings;
            
            Title("настройки");
            return View(new SettingsViewModel { DuplicateMessagesToEmail = seetings .DuplicateMessagesToEmail, NotifyByEmailIfAnybodyAddedMyWishBook = seetings.NotifyByEmailIfAnybodyAddedMyWishBook});
        }

        [POST("settings/update/notifications")]
        public ActionResult UpdateSettings(SettingsViewModel notificationSettings)
        {
            var user = _users.GetById(UserId);
            user.Settings.NotificationSettings = notificationSettings.GetNotificationSettings();

            _users.Save(user);

            return Json("");
        }


        [GET("message/to/{recipientId}")]
        public ActionResult MessageTo(string recipientId)
        {
            var thread = _threads.GetFor(UserId, recipientId);
            if (thread == null)
            {
                var user = _users.GetById(UserId);
                var recipient = _users.GetById(recipientId);
                thread = new Thread(UserId, user.FullName, recipientId, recipient.FullName);
                _threads.Save(thread);
            }
        
            return RedirectToAction("ViewThread", new { threadId = thread.Id });
        }

        [POST("set-news-status-to-read")]
        public ActionResult UpdateUserNewsStatus(string id)
        {
            var user = _users.GetById(UserId);
            user.SetNewsReadStatusTrue(id);
            _users.Save(user);

            return Json("");
        }

        [GET("thread/view/{threadId}")]
        public async Task<ActionResult> ViewThread(string threadId)
        {
            var thread = _threads.GetById(threadId);
            if (!thread.ContainsUser(UserId))
            {
                return HttpNotFound();
            }
            var me = _users.GetById(UserId);
            var recipient = _users.GetById(thread.OwnerId == UserId ? thread.RecipientId : thread.OwnerId);
            var model = new MessagingThreadViewModel(thread, me, recipient);

            _asyncTaskScheduler.StartSetThreadIsRead(UserId, threadId);

            Title("Сообщения от " + recipient.FullName);
            return View("Messages", model);
        }


        [POST("thread/post")]
        public async Task<ActionResult> PostToThread(string threadId, string content)
        {
            var thread = _threads.GetById(threadId);
            if (thread != null && thread.ContainsUser(UserId))
            {
                _threads.PostToThread(threadId, UserId, content);
            }
            else
            {
                return HttpNotFound();
            }
            var sendToUserId = thread.OwnerId == UserId ? thread.RecipientId : thread.OwnerId;

            EmailUsermessage(sendToUserId, content);

            _asyncTaskScheduler.StartUpdateUnreadMessages(threadId, sendToUserId);

            var model = new MessageViewModel();
            model.Init(UserId, content, DateTime.Now.ToString("o"), false);
            var callbackModel = new MessageViewModel();
            callbackModel.Init(UserId, content, DateTime.Now.ToString("o"), true, thread.OwnerId == UserId ? thread.OwnerName : thread.RecipientName);
            ThreadHub.HubContext.Clients.Group(threadId).messageSent(callbackModel);
            NotificationsHub.SendGenericText(sendToUserId, ((UserIdentity)User.Identity).User.FullName, content);
            return Json(model);
        }



        [POST("get-user-books")]
        [AllowAnonymous]
        public ActionResult GetUserBooks(string userId)
        {
            var books = _books.GetUserBooks(userId).Select(x => new BookViewModel(x));
            return Json(books);
        }

        [POST("get-user-wish-books")]
        [AllowAnonymous]
        public ActionResult GetUserWishBooks(string userId)
        {
            var books = _wishBooks.GetUserBooks(userId).Select(x => new BookViewModel(x));
            return Json(books);
        }

        public ActionResult AdjustReputation(string id, int value)
        {
            var user = _users.GetById(id);
            if (!user.Votes.ContainsKey(UserId))
            {
                user.Votes.Add(UserId, 0);
            }

            if (user.Votes[UserId] != 0)
            {
                user.SetVote(UserId, 0);
            }
            else
            {
                user.SetVote(UserId, value);
            }

            var summaryVotes = user.Votes.Values.Sum(x => x);
            var canIncrease = user.GetVote(id, UserId) <= 0;
            var canDecrease = user.GetVote(id, UserId) >= 0;

            _users.Save(user);

            return Json(new { canIncrease = canIncrease, canDecrease = canDecrease, summaryVotes = summaryVotes });
        }


        [POST("get-new-books-count")]
        public ActionResult GetNewBooksCount()
        {
            if (!IsShellUser)
            {
                 var user = _users.GetById(UserId);
                return Json(new { Result = user.Inbox.Count(e => !e.Viewed) });
            }
            //Todo: Realize logic for shell user
            return Json(new { Result = 0 });
        }

        [POST("get-unread-news-count")]
        public ActionResult GetUnreadNewsCount()
        {
            if (!IsShellUser)
            {
                var user = _users.GetById(UserId);
                return Json(new { Result = user.News.Count(n => !n.WasRead) });
            }
            //Todo: Realize logic for shell user
            return Json(new { Result = 0 });
        }

        [POST("get-new-messages-count")]
        public ActionResult ThreadsWithUnreadMessages()
        {
            if (!IsShellUser)
            {
                var user = _users.GetById(UserId);
                return Json(new { Result = user.ThreadsWithUnreadMessages.Count });
            }

            //Todo: Realize logic for shell user
            return Json(new { Result = 0 });          
        }

        [POST]
        public JsonResult UploadImage(HttpPostedFileBase uploadedFile)
        {
            var cloudinary = new CloudinaryDotNet.Cloudinary(_settings.CloudinaryUrl);
            bool isValidImage;

            if (uploadedFile != null)
            {
                isValidImage = uploadedFile.IsValidImage(250, 250);
            }
            else
            {
                // TODO: actions if close button pressed
                return null;
            }

            if (isValidImage)
            {
                uploadedFile.InputStream.Seek(0, SeekOrigin.Begin);

                var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
                {
                    File = new CloudinaryDotNet.Actions.FileDescription("filename", uploadedFile.InputStream),
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                string avatarUrl = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(500).Height(500).Crop("limit")).BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));

                return Json(new { avatarUrl = avatarUrl, avatarId = uploadResult.PublicId, avatarFormat = uploadResult.Format });
            }

            return Json(new { error = "Файл загруженный вами не является изображением или его размеры слишком малы" });
        }

        [POST]
        public JsonResult UploadShellImage(HttpPostedFileBase uploadedFile)
        {
            //var cloudinary = new CloudinaryDotNet.Cloudinary(ConfigurationManager.AppSettings.Get("cloudinary_url"));
            //bool isValidImage;

            //if (uploadedFile != null)
            //{
            //    isValidImage = uploadedFile.IsValidImage(250, 250);
            //}
            //else
            //{
            //    // TODO: actions if close button pressed
            //    return null;
            //}

            //if (isValidImage)
            //{
            //    uploadedFile.InputStream.Seek(0, SeekOrigin.Begin);

            //    var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            //    {
            //        File = new CloudinaryDotNet.Actions.FileDescription("filename", uploadedFile.InputStream),
            //    };

            //    var uploadResult = cloudinary.Upload(uploadParams);
            //    string avatarUrl = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(500).Height(500).Crop("limit")).BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));

            //    return Json(new { avatarUrl = avatarUrl, avatarId = uploadResult.PublicId, avatarFormat = uploadResult.Format });
            //}

            return Json(new { error = "Файл загруженный вами не является изображением или его размеры слишком малы" });
        }

        [POST]
        public JsonResult ResizeAvatar(string avatarId, string avatarFormat, string x, string y, string width, string height)
        {
            var user = _users.GetById(UserId);
            var cloudinary = new CloudinaryDotNet.Cloudinary(_settings.CloudinaryUrl);
            string realAvatarUrl = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(500).Height(500).Crop("limit").Chain().X(x).Y(y).Width(width).Height(height).Crop("crop")).BuildUrl(String.Format("{0}.{1}", avatarId, avatarFormat));

            _cloudinaryImages.AddImage(new CloudinaryImage() { Id = ObjectId.GenerateNewId().ToString(), ImageId = avatarId, ImageUrl = realAvatarUrl });
            user.AvatarUrl = realAvatarUrl;
            _users.Save(user);

            return Json(new { url = realAvatarUrl });
        }

        private void EmailUsermessage(string recipientId, string message)
        {
            var recipient = _users.GetById(recipientId);
            var settings = recipient.Settings.NotificationSettings;

            if (settings.DuplicateMessagesToEmail)
            {
                _mailService.EmailUserMessage(message, _users.GetById(UserId), recipient);
            }
        }
    }
}
