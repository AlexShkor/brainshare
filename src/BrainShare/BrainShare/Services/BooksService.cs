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

        public IEnumerable<Book> GetByIds(IEnumerable<string> ids)
        {
            return Items.Find(Query<Book>.In(x => x.Id, ids));
        }

        public IEnumerable<Book> GetUserWantedBooks(string userId)
        {
            return Items.Find(Query.EQ("Lookers", userId));
        }
    }
}