using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Hubs;
using BrainShare.Services;
using BrainShare.ViewModels;
using MongoDB.Bson;
using Thread = BrainShare.Documents.Thread;
using BrainShare.Extensions;


namespace BrainShare.Controllers
{
    [System.Web.Mvc.Authorize]
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        private readonly BooksService _books;
        private readonly WishBooksService _wishBooks;
        private readonly UsersService _users;
        private readonly ThreadsService _threads;
        private readonly CloudinaryImagesService _cloudinaryImages;

        public ProfileController(BooksService books, UsersService users, ThreadsService threads, WishBooksService whishBooks, CloudinaryImagesService cloudinaryImages)
        {
            _books = books;
            _users = users;
            _threads = threads;
            _wishBooks = whishBooks;
            _cloudinaryImages = cloudinaryImages;
        }

        public ActionResult Index()
        {
            var user = _users.GetById(UserId);
            Title(user.FullName + " мой аккаунт");
            var model = new MyProfileViewModel(user);

            return View(model);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var user = _users.GetById(UserId);
            var model = new EditProfileViewModel(user);
            return View(model);
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

            return JsonModel(model);
        }

        [GET("view/{id}")]
        public ActionResult ViewUserProfile(string id)
        {
            if (id == UserId)
            {
                return RedirectToAction("Index", "Profile");
            }

            var user = _users.GetById(id);
            var model = new UserProfileModel(user, UserId);

            model.CanIncrease = user.GetVote(id, UserId) <= 0;
            model.CanDecrease = user.GetVote(id, UserId) >= 0;
            model.SummaryVotes = user.GetSummaryVotes();

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
            Title("Входящие");
            var user = _users.GetById(UserId);
            var books = _books.GetByIds(user.Inbox.Select(x => x.BookId)).ToList();
            var users = _users.GetByIds(user.Inbox.Select(x => x.UserId)).ToList();
            var model = new InboxViewModel();
            model.Items = user.Inbox.OrderByDescending(x => x.Created).Select(x =>
                                            new InboxItem(x.Created, books.Find(b => b.Id == x.BookId),
                                                          users.Find(u => u.Id == x.UserId), !x.Viewed)).ToList();
            return View(model);
        }

        [GET("reject/{bookId}/from/{userId}")]
        public ActionResult Reject(string bookId, string userId)
        {
            var user = _users.GetById(UserId);
            user.Inbox.RemoveAll(x => x.BookId == bookId && x.UserId == userId);
            _users.Save(user);
            return RedirectToAction("Inbox");
        }


        [GET("messages")]
        public ActionResult AllMessages()
        {
            var threads = _threads.GetAllForUser(UserId).Where(x => x.Messages.Any()).OrderByDescending(x => x.Messages.Max(m => m.Posted));
            var user = _users.GetById(UserId);
            var model = new AllThreadsViewModel(threads, UserId, user);
            Title("Сообщения");
            return View(model);
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

        [GET("thread/view/{threadId}")]
        public ActionResult ViewThread(string threadId)
        {
            var thread = _threads.GetById(threadId);
            if (!thread.ContainsUser(UserId))
            {
                return HttpNotFound();
            }
            var me = _users.GetById(UserId);
            var recipient = _users.GetById(thread.OwnerId == UserId ? thread.RecipientId : thread.OwnerId);
            var model = new MessagingThreadViewModel(thread, me, recipient);
            SetThreadIsReadAsync(UserId, threadId);
            Title("Сообщения от " + recipient.FullName);
            return View("Messages", model);
        }


        [POST("thread/post")]
        public ActionResult PostToThread(string threadId, string content)
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
            UpdateUnreadMessagesAsync(threadId, sendToUserId);
            var model = new MessageViewModel();
            model.Init(UserId, content, DateTime.Now.ToString("o"), false);
            var callbackModel = new MessageViewModel();
            callbackModel.Init(UserId, content, DateTime.Now.ToString("o"), true, thread.OwnerId == UserId ? thread.OwnerName : thread.RecipientName);
            ThreadHub.HubContext.Clients.Group(threadId).messageSent(callbackModel);
            NotificationsHub.SendGenericText(sendToUserId, ((UserIdentity)User.Identity).User.FullName, content);
            return Json(model);
        }

        private void UpdateUnreadMessagesAsync(string threadId, string toUserId)
        {
            Task.Factory.StartNew(() => _users.AddThreadToUnread(toUserId, threadId));
        }

        private void SetThreadIsReadAsync(string userId, string threadId)
        {
            Task.Factory.StartNew(() => _users.RemoveThreadFromUnread(userId, threadId));
        }

        [POST("get-user-books")]
        public ActionResult GetUserBooks(string userId)
        {
            var books = _books.GetUserBooks(userId).Select(x => new BookViewModel(x));
            return Json(books);
        }

        [POST("get-user-wish-books")]
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
            var user = _users.GetById(UserId);
            //replaces with all book requests, not only new
            return Json(new { Result = user.Inbox.Count });
        }

        [POST("get-new-messages-count")]
        public ActionResult ThreadsWithUnreadMessages()
        {
            var user = _users.GetById(UserId);
            return Json(new { Result = user.ThreadsWithUnreadMessages.Count });
        }

        [POST]
        public JsonResult UploadImage(HttpPostedFileBase uploadedFile)
        {
            var cloudinary = new CloudinaryDotNet.Cloudinary(ConfigurationManager.AppSettings.Get("cloudinary_url"));
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
        public JsonResult ResizeAvatar(string avatarId, string avatarFormat, string x, string y, string width, string height)
        {
            var user = _users.GetById(UserId);
            var cloudinary = new CloudinaryDotNet.Cloudinary(ConfigurationManager.AppSettings.Get("cloudinary_url"));
            string realAvatarUrl = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(500).Height(500).Crop("limit").Chain().X(x).Y(y).Width(width).Height(height).Crop("crop")).BuildUrl(String.Format("{0}.{1}", avatarId, avatarFormat));

            _cloudinaryImages.AddImage(new CloudinaryImage() { Id = ObjectId.GenerateNewId().ToString(), ImageId = avatarId, ImageUrl = realAvatarUrl });
            user.AvatarUrl = realAvatarUrl;
            _users.Save(user);

            return Json(new { url = realAvatarUrl });
        }
    }
}
