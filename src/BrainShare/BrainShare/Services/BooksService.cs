using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver;

namespace BrainShare.Services
{
    public class BooksService : BaseBooksService
    {
        public BooksService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<Book> Items
        {
            get { return Database.Books; }
        }
    }
}