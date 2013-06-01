using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrainShare.Documents;
using BrainShare.Hubs;
using BrainShare.Services;
using BrainShare.ViewModels;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;


namespace BrainShare.Controllers
{
    [System.Web.Mvc.Authorize]
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        private readonly BooksService _books;
        private readonly UsersService _users;
        private readonly ThreadsService _threads;

        public ProfileController(BooksService books, UsersService users, ThreadsService threads)
        {
            _books = books;
            _users = users;
            _threads = threads;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GET("view/{id}")]
        public ActionResult ViewUserProfile(string id)
        {
            var user = _users.GetById(id);
            var model = new UserProfileModel(user, UserId);
            return View(model);
        }

        public ActionResult DontHave(BookViewModel book)
        {
            try
            {
                _users.RemoveFromBooks(book.Id, UserId);
                _books.RemoveOwner(book.Id, UserId);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }

            return Json(new { Success = "OK" });
        }

        [HttpPost]
        public ActionResult DontWant(BookViewModel book)
        {
            try
            {
                _users.RemoveFromWishList(book.Id, UserId);
                _books.RemoveLooker(book.Id, UserId);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }

            return Json(new { Success = "OK" });
        }

        public ActionResult MyBooks()
        {
            var books = _books.GetUserBooks(UserId);
            var model = books.Select(x => new BookViewModel(x)).ToList();
            return View(model);
        }

        public ActionResult WishList()
        {
            var books = _books.GetUserWantedBooks(UserId);
            var model = books.Select(x => new BookViewModel(x)).ToList();
            return View(model);
        }

        public ActionResult Inbox()
        {
            var user = _users.GetById(UserId);
            var books = _books.GetByIds(user.Inbox.Select(x => x.BookId)).ToList();
            var users = _users.GetByIds(user.Inbox.Select(x => x.UserId)).ToList();
            var model = new InboxViewModel();
            model.Items = user.Inbox.OrderBy(x => x.Created).Select(x =>
                                            new InboxItem(x.Created, books.Find(b => b.Id == x.BookId),
                                                          users.Find(u => u.Id == x.UserId))).ToList();
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
            var model = new AllThreadsViewModel(threads, UserId);
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
            var model = new MessageViewModel();
            model.Init(UserId,content,DateTime.Now,false);
            var callbackModel = new MessageViewModel();
            callbackModel.Init(UserId, content, DateTime.Now, true, thread.OwnerId == UserId ? thread.OwnerName : thread.RecipientName);
            ThreadHub.HubContext.Clients.Group(threadId).messageSent(callbackModel);
            return Json(model);
        }

        [POST("getBooks")]
        public ActionResult GetUserBooks(IEnumerable<string> ids)
        {
            var books = _books.GetByIds(ids);
            return Json(new {books = books});
        }
    }
}
