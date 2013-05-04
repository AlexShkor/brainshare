using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrainShare.Documents;
using BrainShare.Services;

namespace BrainShare.Controllers
{
    [Authorize]
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

        public ActionResult DontHave(string id)
        {
            var user = _users.GetById(UserId);
            return RedirectToAction("MyBooks");
        }

        public ActionResult DontWant(string id)
        {
             var user = _users.GetById(UserId);
            return RedirectToAction("WishList");
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
            model.Items = user.Inbox.OrderBy(x=> x.Created).Select(x =>
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
            var threads = _threads.GetAllForUser(UserId).Where(x=> x.Messages.Any()).OrderByDescending(x=> x.Messages.Max(m=> m.Posted));
            var model = new AllThreadsViewModel(threads,UserId);
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
           return RedirectToAction("ViewThread",new {threadId = thread.Id});
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
            var model = new MessagingThreadViewModel(thread, me,recipient);
            return View("Messages",model);
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
            return RedirectToAction("ViewThread", new { threadId = thread.Id });
        }
    }

    public class AllThreadsViewModel
    {
        public List<ThreadItemViewModel> Items { get; set; } 

        public AllThreadsViewModel(IEnumerable<Thread> threads, string me)
        {
            Items = threads.Select(x => new ThreadItemViewModel(x,me)).ToList();
        }
    }

    public class ThreadItemViewModel
    {

        public string ThreadId { get; set; }

        public string To { get; set; }

        public ThreadItemViewModel(Thread thread, string me)
        {
            ThreadId = thread.Id;
            To = thread.GetSecondUserName(me);
        }

    }

    public class MessagingThreadViewModel
    {
        public string ThreadId { get; set; }
        public string RecipientId { get; set; }
        public string RecipientName { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public MessagingThreadViewModel(Thread thread, User me, User recipient)
        {
            ThreadId = thread.Id;
            RecipientId = recipient.Id;
            RecipientName = recipient.FullName;
            Messages = thread.Messages.Select(x => new MessageViewModel(x, recipient)).ToList();
        }
    }

    public class MessageViewModel
    {
        public string From { get; set; }

        public string Content { get; set; }

        public string Posted { get; set; }

        public string Class { get; set; }

        public MessageViewModel(Message message, User recipient)
        {
            var notMe = message.UserId == recipient.Id;
            From = notMe ? recipient.FullName : "Я";
            Class = notMe ? "" : "pull-right";
            Posted = message.Posted.ToRelativeDate();
            Content = message.Content;
        }
    }

    public class UserProfileModel 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMe { get; set; }

        public UserProfileModel(User user, string myId)
        {
            Id = user.Id;
            Name = user.FullName;
            IsMe = user.Id == myId;
        }
    }
}
