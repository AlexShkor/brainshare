using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrainShare.Documents;
using BrainShare.Hubs;
using BrainShare.Services;
using BrainShare.ViewModels;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
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

        public ProfileController(BooksService books, UsersService users, ThreadsService threads, WishBooksService whishBooks)
        {
            _books = books;
            _users = users;
            _threads = threads;
            _wishBooks = whishBooks;
        }

        public ActionResult Index()
        {
            var user = _users.GetById(UserId);
            Title(user.FullName + " мой аккаунт");
            var model = new MyProfileViewModel(user);

            return View(model);
        }

        [GET("view/{id}")]
        public ActionResult ViewUserProfile(string id)
        {
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
            NotificationsHub.SendGenericText(sendToUserId, User.Identity.Name, content);
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

        [POST]
        public JsonResult UploadImage(HttpPostedFileBase uploadedFile)
        {
            var cloudinary = new CloudinaryDotNet.Cloudinary(ConfigurationManager.AppSettings.Get("cloudinary_url"));
            bool isValidImage;

            if (uploadedFile != null)
            {
                isValidImage = uploadedFile.IsValidImage();
            }
            else
            {
                // TODO: actions if close button pressed
                return null;
            }

            if (isValidImage)
            {
                var user = _users.GetById(UserId);
                uploadedFile.InputStream.Seek(0, SeekOrigin.Begin);

                if (user.AvatarId != null)
                {
                    cloudinary.Destroy(new DeletionParams(user.AvatarId));
                }

                var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
                 {
                     File = new CloudinaryDotNet.Actions.FileDescription("filename", uploadedFile.InputStream),
                 };

                var uploadResult = cloudinary.Upload(uploadParams);
                string avatarUrl = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(500).Height(500).Crop("limit")).BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));

                user.AvatarId = uploadParams.PublicId;
                _users.Save(user);

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
            //string realAvatarUrl = cloudinary.Api.UrlImgUp.Transform(new CloudinaryDotNet.Transformation().Width(250).Height(250).Crop("fill").Width(500).Height(500).Crop("limit").Chain().X(x).Y(y).Width(width).Height(height).Crop("crop")).BuildUrl(String.Format("{0}.{1}", avatarId, avatarFormat));

            user.AvatarUrl = realAvatarUrl;
            _users.Save(user);

            return Json(new { url = realAvatarUrl });
        }

        [POST("get-new-books-count")]
        public ActionResult GetNewBooksCount()
        {
            var user = _users.GetById(UserId);
            return Json(new { Result = user.Inbox.Count(x => !x.Viewed) });
        }

        [POST("get-new-messages-count")]
        public ActionResult ThreadsWithUnreadMessages()
        {
            var user = _users.GetById(UserId);
            return Json(new { Result = user.ThreadsWithUnreadMessages.Count });
        }
    }


    public class MyProfileViewModel
    {
        public MyProfileViewModel()
        {

        }

        public MyProfileViewModel(User user)
        {
            Name = user.FullName;
            AvatarUrl = user.AvatarUrl ?? Constants.DefaultAvatarUrl;
        }

        public string Name { get; set; }
        public string AvatarUrl { get; set; }

        public HttpPostedFileBase UploadedFile { get; set; }
        public int Id { get; set; }
    }
}
