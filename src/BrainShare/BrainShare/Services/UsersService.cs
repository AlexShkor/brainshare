﻿using System.Collections.Generic;
using BrainShare.Documents;
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

        public bool CheckLikedUsers(string whoLikes, string whoLiked)
        {
            var user = Items.FindOne(Query<User>.EQ(x => x.Id, whoLikes));
            return !user.RaitedUserIds.Contains(whoLiked);
        }

        public void IncreaseReputation(string id, string increaserId)
        {
            var user = Items.FindOne(Query<User>.EQ(x => x.Id, id));
            user.Votes += 1;
            Items.Save(user);

            var increaser = Items.FindOne(Query<User>.EQ(x => x.Id, increaserId));
            increaser.RaitedUserIds.Add(id);
            Items.Save(increaser);
        }

        public void ReduceReputation(string id, string decreaserId)
        {
            var user = Items.FindOne(Query<User>.EQ(x => x.Id, id));
            user.Votes -= 1;
            Items.Save(user);

            var decreaser = Items.FindOne(Query<User>.EQ(x => x.Id, decreaserId));
            decreaser.RaitedUserIds.Add(id);
            Items.Save(decreaser);
        }
    }
}