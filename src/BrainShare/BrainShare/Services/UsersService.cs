using System;
using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents;
using BrainShare.Facebook;
using BrainShare.Infostructure.Filters;
using BrainShare.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BrainShare.Services
{
    public class UsersService : DocumentsServiceFiltered<User,UsersFilter>
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
            return Items.FindOne(Query<User>.EQ(x => x.Email, email.ToLower()));
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

        public void UpdateRequestViewed(string userId, string bookId, string requestFromUserId)
        {
            Items.Update(Query.And(Query<User>.EQ(x => x.Id, userId), Query<User>.ElemMatch(x => x.Inbox,
                (el) => el.And(el.EQ(x => x.BookId, bookId), el.EQ(x => x.User.UserId, requestFromUserId)))),
                Update.Set("Inbox.$.Viewed", true));
        }

        public void AddThreadToUnread(string userId, string threadId)
        {
            Items.Update(
                Query<User>.EQ(x => x.Id, userId),
                Update<User>.AddToSet(x=> x.ThreadsWithUnreadMessages, threadId));
        }

        public void RemoveThreadFromUnread(string userId, string threadId)
        {
            Items.Update(
                Query<User>.EQ(x => x.Id, userId),
                Update<User>.Pull(x => x.ThreadsWithUnreadMessages, threadId));
        }

        public IEnumerable<User> GetLast(int count)
        {
            return Items.FindAll().SetSortOrder(SortBy<User>.Descending(x => x.Registered)).SetLimit(count);
        }

        protected override IEnumerable<IMongoQuery> BuildFilterQuery(UsersFilter filter)
        {
            if (filter.FirstName.HasValue())
            {
                yield return Query<User>.Matches(x => x.FirstName, new BsonRegularExpression(filter.FirstName, "i"));
            }
            if (filter.LastName.HasValue())
            {
                yield return Query<User>.Matches(x => x.LastName, new BsonRegularExpression(filter.FirstName, "i"));
            }
        }
    }
}