using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class ActivityFeed
    {
        [BsonId]
        public string Id { get; set; }

        public ActivityTypeEnum ActivityType { get; set; }

        public DateTime Created { get; set; }

        public List<ActivityFeedItem> Items { get; set; }

        public ActivityFeed()
        {
            Items = new List<ActivityFeedItem>();
        }

        public static ActivityFeed BookAdded(string bookId, string bookTitle, string userId, string userName)
        {
            return OneBook(bookId, bookTitle, userId, userName, ActivityTypeEnum.BookAdded);
        }

        public static ActivityFeed BookWanted(string bookId, string bookTitle, string userId, string userName)
        {
            return OneBook(bookId, bookTitle, userId, userName, ActivityTypeEnum.BookWanted);
        }


        public static ActivityFeed BooksExchanged(Book book1, User user1, Book book2, User user2)
        {
            var result = OneBook(book1.Id, book1.Title, user1.Id, user1.FullName, ActivityTypeEnum.BooksExchanged);
            result.Items.Add(new ActivityFeedItem
                {
                    UserId = user2.Id,
                    UserName = user2.FullName,
                    BookId = book2.Id,
                    BookTitle = book2.Title
                });
            return result;
        }

        private  static  ActivityFeed OneBook(string bookId, string bookTitle, string userId, string userName, ActivityTypeEnum activityType)
        {
            var result = new ActivityFeed
            {
                ActivityType = activityType,
                Id = ObjectId.GenerateNewId().ToString(),
                Created = DateTime.Now
            };
            result.Items.Add(new ActivityFeedItem
            {
                UserId = userId,
                UserName = userName,
                BookId = bookId,
                BookTitle = bookTitle,
            });
            return result;
        }
    }
}