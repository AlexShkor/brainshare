using System;
using System.Collections.Generic;
using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BrainShare.Services
{
    public abstract class BaseBooksService : DocumentsServiceFiltered<Book,BooksFilter>
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

        public IEnumerable<Book> GetPaged(string query, int skip, int limit)
        {
            if (query.HasValue())
            {
                return
                    Items.Find(Query<Book>.Matches(x => x.Title, new BsonRegularExpression(query,"i"))).SetSortOrder(SortBy<Book>.Descending(x=> x.Added)).SetSkip(skip).SetLimit
                        (limit);
            }
            else
            {
                return
                    Items.FindAll().SetSortOrder(SortBy<Book>.Descending(x => x.Added)).SetSkip(skip).SetLimit
                        (limit);
            }
        }

        protected override IEnumerable<IMongoQuery> BuildFilterQuery(BooksFilter filter)
        {
            if (filter.Title.HasValue())
            {
                yield return Query<Book>.Matches(x => x.Title, new BsonRegularExpression(filter.Title,"i"));
            }
        }

        protected override IMongoSortBy BuildSortExpression(BooksFilter filter)
        {
            return SortBy<Book>.Descending(x => x.Added);
        }

        public IEnumerable<Book> GetLast(int count)
        {
            return Items.FindAll().SetSortOrder(SortBy<Book>.Descending(x => x.Added)).SetLimit(count);
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