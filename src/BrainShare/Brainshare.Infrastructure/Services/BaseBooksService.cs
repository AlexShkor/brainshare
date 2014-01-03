using System.Collections.Generic;
using BinaryAnalysis.UnidecodeSharp;
using BrainShare.Domain.Documents;
using BrainShare.Infrastructure.Infrastructure.Filters;
using BrainShare.Infrastructure.Mongo;
using BrainShare.Utils.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;

namespace Brainshare.Infrastructure.Services
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
            if (filter.Author.HasValue())
            {
                yield return Query<Book>.Matches(x=> x.Authors, new BsonRegularExpression(filter.Author,"i"));
            }
            if (filter.ISBN.HasValue())
            {
                yield return Query<Book>.In(x=> x.ISBN, filter.ISBN.Split(',').Select(i => i.Trim()));
            }
            if (filter.Title.HasValue())
            {
                yield return Query<Book>.Matches(x => x.Title, new BsonRegularExpression(filter.Title,"i"));
            }
            if (filter.UserName.HasValue())
            {
                yield return Query<Book>.Matches(x => x.UserData.UserName, new BsonRegularExpression(filter.UserName,"i"));
            }
            if (filter.Language.HasValue())
            {
                yield return Query<Book>.EQ(x => x.Language, filter.Language);
            }
            if (filter.Location.HasValue())
            {
               yield return Query.Or(
                    Query<Book>.Matches(x => x.UserData.Address.Locality, new BsonRegularExpression(filter.Location, "i")),
                    Query<Book>.Matches(x => x.UserData.Address.Country, new BsonRegularExpression(filter.Location, "i")),
                    Query<Book>.Matches(x => x.UserData.Address.Formatted, new BsonRegularExpression(filter.Location, "i")),
                    Query<Book>.Matches(x => x.UserData.Address.Original, new BsonRegularExpression(filter.Location, "i")),
                    Query<Book>.Matches(x => x.UserData.Address.Locality, new BsonRegularExpression(filter.Location.Unidecode(), "i")),
                    Query<Book>.Matches(x => x.UserData.Address.Country, new BsonRegularExpression(filter.Location.Unidecode(), "i")),
                    Query<Book>.Matches(x => x.UserData.Address.Formatted, new BsonRegularExpression(filter.Location.Unidecode(), "i")),
                    Query<Book>.Matches(x => x.UserData.Address.Original, new BsonRegularExpression(filter.Location.Unidecode(), "i"))
                    );
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

        public Book GetByOzBookId(string ozBookId)
        {
            return Items.FindOne(Query<Book>.EQ(b => b.OzBookId, ozBookId));
        }

        public Book GetUserBook(string googleBookId, string userId)
        {
            return
                Items.FindOne(Query.And(Query<Book>.EQ(x => x.UserData.UserId, userId),
                                        Query<Book>.EQ(b => b.GoogleBookId, googleBookId)));
        }

        public List<Book> GetBooksByISBN(IEnumerable<string> ISBN)
        {
            return
                Items.Find(Query<Book>.In(b => b.ISBN, ISBN)).ToList();
        }

        public void Remove(string id)
        {
            Items.Remove(Query<Book>.EQ(x => x.Id, id));
        }

        public IEnumerable<string> GetOzIds(string userId)
        {
            return Items.Find(
                Query.And(
                     Query<Book>.EQ(x => x.UserData.UserId, userId),
                     Query<Book>.EQ(b => b.FromOzBy, true),
                     Query<Book>.NE(b => b.OzBookId, null)
                 ))
                 .SetFields(Fields<Book>.Include(b => b.OzBookId))
                 .ToList()
                 .Select(e => e.OzBookId);
        }

        public IEnumerable<string> GetOzIdsWithEmptyIsbn()
        {
            return Items.Find(
                Query.And(
                     Query<Book>.EQ(b => b.FromOzBy, true),
                     Query<Book>.Size(b => b.ISBN, 0)
                 ))
                 .SetFields(Fields<Book>.Include(b => b.OzBookId))
                 .ToList()
                 .Select(e => e.OzBookId);
        }

        public IEnumerable<string> GetGoogleIds(string userId)
        {
            return Items.Find(
                Query.And(
                     Query<Book>.EQ(x => x.UserData.UserId, userId),
                     Query<Book>.NE(b => b.GoogleBookId, null)
                 ))
                 .SetFields(Fields<Book>.Include(b => b.Id))
                 .ToList()
                 .Select(e => e.Id);
        }

    }
}