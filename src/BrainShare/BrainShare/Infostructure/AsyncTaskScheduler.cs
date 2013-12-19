using System.Threading.Tasks;
using BrainShare.Documents;
using BrainShare.Services;
using BrainShare.Utilities;

namespace BrainShare.Infostructure
{
    public class AsyncTaskScheduler
    {
        private readonly UsersService _users;
        private readonly BooksService _books;
        private readonly WishBooksService _wishBooks;
        private readonly ActivityFeedsService _feeds;
        private readonly NewsService _newsService;
        private readonly MailService _mailService;

        public AsyncTaskScheduler(WishBooksService wishBooks, ActivityFeedsService feeds, UsersService users, BooksService books, NewsService newsService,MailService mailService)
        {
            _wishBooks = wishBooks;
            _feeds = feeds;
            _users = users;
            _books = books;
            _newsService = newsService;
            _mailService = mailService;
        }

        public Task StartEmailSendSearchingUsersTask(User owner, Book newBook)
        {
            return Task.Factory.StartNew(() =>
            {
                var wishBooks = _wishBooks.GetBooksByISBN(newBook.ISBN);
                foreach (var wishBook in wishBooks)
                {
                    var userData = wishBook.UserData;
                    var user = _users.GetById(userData.UserId);

                    if (user.Settings.NotificationSettings.NotifyByEmailIfAnybodyAddedMyWishBook)
                    {
                        _mailService.EmailUserHaveSearechedBook(owner, user, wishBook);
                    }
                }
            });
        }

        public Task StartNewsSendSearchingUsersTask(User owner, Book newBook)
        {
            return Task.Factory.StartNew(() =>
            {
                var wishBooks = _wishBooks.GetBooksByISBN(newBook.ISBN);
                foreach (var wishBook in wishBooks)
                {
                    var userData = wishBook.UserData;
                    var news = new News(
                        NewsMaker.UserHaveBookMessage(owner.FullName, owner.Id, newBook.Title, newBook.Id),
                        "Информация по разыкиваемой книге");
                    _newsService.Save(news);

                    var taker = _users.GetById(userData.UserId);
                    taker.AddNews(news.Id);
                    _users.Save(taker);
                }
            });
        }

        public Task StartSaveFeedTask (ActivityFeed feed)
        {
            return Task.Factory.StartNew(() => _feeds.Save(feed));
        }

        public Task StartUpdateRequestViewed(string userId, string bookId, string requestFromUserId)
        {
           return Task.Factory.StartNew(() => _users.UpdateRequestViewed(userId, bookId, requestFromUserId));
        }

        public Task StartUpdateUnreadMessages(string threadId, string toUserId)
        {
           return Task.Factory.StartNew(() => _users.AddThreadToUnread(toUserId, threadId));
        }

        public Task StartSetThreadIsRead(string userId, string threadId)
        {
            return Task.Factory.StartNew(() => _users.RemoveThreadFromUnread(userId, threadId));
        }
    }
}