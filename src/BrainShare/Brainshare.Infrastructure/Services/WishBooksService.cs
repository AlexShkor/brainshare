using BrainShare.Domain.Documents;
using BrainShare.Infrastructure.Mongo;
using Brainshare.Infrastructure.Services;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BrainShare.Services
{
    public class WishBooksService : BaseBooksService
    {
        public WishBooksService(MongoDocumentsDatabase database)
            : base(database)
        {
        }

        protected override MongoCollection<Book> Items
        {
            get { return Database.WishBooks; }
        }

        public void Delete(string userId, string googleBookId)
        {
            Items.Remove(Query.And(Query<Book>.EQ(x => x.UserData.UserId, userId),
                                   Query<Book>.EQ(b => b.GoogleBookId, googleBookId)));
        }
  }
}