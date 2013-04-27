using System.Collections.Generic;
using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BrainShare.Services
{
    public class BooksService : DocumentsService<Book>
    {
        public BooksService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<Book> Items
        {
            get { return Database.Books; }
        }

        public IEnumerable<Book> GetUserBooks(string userId)
        {
            return Items.Find(Query.EQ("Owners", userId));
        }
    }

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
    }
}