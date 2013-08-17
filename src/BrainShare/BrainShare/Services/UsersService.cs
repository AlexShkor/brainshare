using System;
using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents;
using BrainShare.Facebook;
using BrainShare.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BrainShare.Services
{
    public class UsersService : DocumentsService<User>
    {
        public UsersService(MongoDocumentsDatabase database)
            : base(database)
        {
        }

        protected override MongoCollection<User> Items
        {
            get { return Database.Users; }
        }

        public User GetUserByEmail(string email)
        {
            return Items.FindOne(Query<User>.EQ(x => x.Email, email));
        }

        public User GetByCredentials(string email, string password)
        {
            return
                Items.FindOne(Query.And(Query<User>.EQ(x => x.Email, email),
                                        Query<User>.EQ(x => x.Password, password)));
        }

        public User GetByFacebookId(string facebookId)
        {
            return
                Items.FindOne(Query<User>.EQ(x => x.FacebookId, facebookId));
        }

        public void AddUser(User user)
        {
            Items.Save(user);
        }

        public IEnumerable<User> GetOwners(string id)
        {
            return Items.Find(Query.EQ("Books", id));
        }

        public IEnumerable<User> GetByIds(IEnumerable<string> ids)
        {
            return Items.Find(Query<User>.In(x => x.Id, ids));
        }

        public IEnumerable<FacebookFriend> GetExistingUsersIds(IEnumerable<string> ids)
        {
            return Items.Find(Query<User>.In(x => x.FacebookId, ids)).Select(x => new FacebookFriend()
                                                                                      {
                                                                                          Id = x.Id,
                                                                                          FacebookId = x.FacebookId
                                                                                      });
        }

        public void RemoveFromWishList(string bookId, string userId)
        {
            var user = Items.FindOne(Query<User>.EQ(x => x.Id, userId));
            user.WishList.Remove(bookId);
            Items.Save(user);
        }

        public void RemoveFromBooks(string bookId, string userId)
        {
            var user = Items.FindOne(Query<User>.EQ(x => x.Id, userId));
            user.Books.Remove(bookId);
            Items.Save(user);
        }

        public void UpdateRequestViewed(string userId, string bookId, string requestFromUserId)
        {
            Items.Update(Query.And(Query<User>.EQ(x => x.Id, userId), Query<User>.ElemMatch(x => x.Inbox,
                (el) => el.And(el.EQ(x => x.BookId, bookId), el.EQ(x => x.UserId, requestFromUserId)))),
                Update.Set("Inbox.$.Viewed", true));
        }
    }
}