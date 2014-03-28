using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Mongo;
using MongoDB.Driver;

namespace Brainshare.Infrastructure.Services
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