using System;
using System.Collections.Generic;
using System.Linq;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using BrainShare.Infrastructure.Infrastructure.Filters;
using BrainShare.Infrastructure.Mongo;
using BrainShare.Utils.Extensions;
using Brainshare.Infrastructure.Facebook;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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

        public User GetUserByLoginServiceInfo(LoginServiceTypeEnum loginServiceType, string serviceId)
        {
            switch (loginServiceType)
            {
                case LoginServiceTypeEnum.Facebook:
                    return GetUserByFbId(serviceId);
                case LoginServiceTypeEnum.Vk:
                    return GetUserByVkId(serviceId);
                case LoginServiceTypeEnum.Email:
                    return GetUserByEmail(serviceId);
            }
            return null;
        }


        public void AddUser(User user)
        {
            Items.Save(user);
        }


        public User GetUserByEmail(string email)
        {
            return Items.FindOne(Query<User>.EQ(x => x.Email, email));
        }

        public User GetUserByVkId(string id)
        {
            return Items.FindOne(Query<User>.EQ(x => x.VkId, id));
        }

        public User GetUserByFbId(string id)
        {
            return Items.FindOne(Query<User>.EQ(x => x.FacebookId, id));
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

        public void SetOnlineStatus(string userId, bool isOnline)
        {
            Items.Update(Query<User>.EQ(x => x.Id, userId), Update<User>.Set(x => x.IsOnline, isOnline));
        }

        public void SetLastVisitedDate(DateTime date, string userId)
        {
            Items.Update(Query<User>.EQ(x => x.Id, userId), Update<User>.Set(x => x.LastVisited, date));
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
                yield return Query<User>.Matches(x => x.LastName, new BsonRegularExpression(filter.LastName, "i"));
            }
            if (filter.Country.HasValue())
            {
                yield return Query<User>.Matches(x => x.Address.Country, new BsonRegularExpression(filter.Country, "i"));
            }
        }
    }
}