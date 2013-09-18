using System.Collections.Generic;
using BrainShare.Documents;
using BrainShare.Mongo;
using Elmah.ContentSyndication;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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

    public abstract class BaseBooksService : DocumentsService<Book>
    {
        protected BaseBooksService(MongoDocumentsDatabase database)
            : base(database)
        {
        }

        public IEnumerable<Book> GetUserBooks(string userId)
        {
            return Items.Find(Query<Book>.EQ(x=> x.UserData.UserId, userId));
        }

        public IEnumerable<Book> GetByIds(IEnumerable<string> ids)
        {
            return Items.Find(Query<Book>.In(b => b.Id, ids));
        }

        public IEnumerable<Book> GetPaged(int skip, int limit)
        {
            return Items.FindAll().SetSkip(skip).SetLimit(limit);
        }

        public IEnumerable<Book> GetByGoogleBookId(string googleBookId)
        {
            return Items.Find(Query<Book>.EQ(b => b.GoogleBookId, googleBookId));
        }

        public Book GetUserBook(string googleBookId, string userId)
        {
            return
                Items.FindOne(Query.And(Query<Book>.EQ(x => x.UserData.UserId, userId),
                    Query<Book>.EQ(b => b.GoogleBookId, googleBookId)));
        }

        public void Remove(string id)
        {
            Items.Remove(Query<Book>.EQ(x => x.Id, id));
        }
    }
}