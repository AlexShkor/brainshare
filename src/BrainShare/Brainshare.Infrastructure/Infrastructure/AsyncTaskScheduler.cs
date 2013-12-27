﻿using System.Threading.Tasks;
using BrainShare.Domain.Documents;
using BrainShare.Infrastructure.Services;
using BrainShare.Services;
using BrainShare.Infrastructure.Utilities;
using Brainshare.Infrastructure.Hubs;
using Brainshare.Infrastructure.Services;

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
                    SendEmailSearhingUser(wishBook, owner);
                }
            });
        }

        public Task UserHaveNewBookNotifyer(User owner, Book newBook)
        {
            return Task.Factory.StartNew(() =>
            {
                var wishBooks = _wishBooks.GetBooksByISBN(newBook.ISBN);

                string title = "Информация по разыкиваемой книге";
                var content = NewsMaker.UserHaveBookMessage(owner.FullName, owner.Id, newBook.Title, newBook.Id); 

                var news = new News(content,title);
                _newsService.Save(news);

                foreach (var wishBook in wishBooks)
                {
                    AddNews(wishBook.UserData.UserId,news.Id);  
                    NotificationsHub.SendGenericText(wishBook.UserData.UserId,title,content);                    
                }

                news = new News(content, "Новости от " + owner.FullName);
                _newsService.Save(news);

                foreach (var followerId in owner.Followers)
                {
                    AddNews(followerId,news.Id);
                }
            });
        }

        public Task UserSearchedForNewBookNotifyer(User searcher, Book searchedBook)
        {
            return Task.Factory.StartNew(() =>
            {
                var existingBooks = _books.GetBooksByISBN(searchedBook.ISBN);

                string title = "Пользователь ищет имеющуюся книгу";
                var content = NewsMaker.UserSearchedBookMessage(searcher.FullName,searchedBook.Title);

                var news = new News(content, title);
                _newsService.Save(news);

                foreach (var existingBook in existingBooks)
                {
                    //Todo: notifications policy
                    AddNews(existingBook.UserData.UserId,news.Id);
                    NotificationsHub.SendGenericText(existingBook.UserData.UserId, title, content);
                }

                news = new News(content, "Новости от " + searcher.FullName);
                _newsService.Save(news);

                foreach (var followerId in searcher.Followers)
                {
                    AddNews(followerId, news.Id);
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

        #region helpers

        private void AddNews(string userId, string newsId)
        {
            var taker = _users.GetById(userId);
            taker.AddNews(newsId);
            _users.Save(taker);
        }

        private void SendEmailSearhingUser(Book wishBook, User owner)
        {
            var userData = wishBook.UserData;
            var user = _users.GetById(userData.UserId);

            if (user.Settings.NotificationSettings.NotifyByEmailIfAnybodyAddedMyWishBook)
            {
                _mailService.EmailUserHaveSearechedBook(owner, user, wishBook);
            }
        }

        #endregion
    }
}