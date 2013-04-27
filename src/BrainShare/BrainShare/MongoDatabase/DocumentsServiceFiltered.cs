using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MvcMusicStore.MongoDB
{
    public abstract class DocumentsServiceFiltered<T, TFilter> : DocumentsService<T> where TFilter : BaseFilter
    {
        protected DocumentsServiceFiltered(MongoDocumentsDatabase database)
            : base(database)
        {
        }

        public IEnumerable<T> GetByFilter(TFilter filter)
        {
            var queries = BuildFilterQuery(filter).ToList();
            //if filter was not applied we not return all documents, we just return empty list
            if (!queries.Any() && filter.PagingInfo == null && filter.Ordering == null)
                return new List<T>();
            var cursor = queries.Any() ? Items.FindAs<T>(Query.And(queries.ToArray())) : Items.FindAllAs<T>();
            var sortOrder = BuildSortExpression(filter);
            if (sortOrder != SortBy.Null)
                cursor.SetSortOrder(sortOrder);
            if (filter.ExcludeFields.Count > 0)
                cursor.SetFields(Fields.Exclude(filter.ExcludeFields.ToArray()));
            if (filter.IsPagingEnabled)
            {
                var pagingInfo = filter.PagingInfo;
                cursor.SetSkip(pagingInfo.Skip);
                cursor.SetLimit(pagingInfo.Take);
                pagingInfo.TotalCount = cursor.Count();
                var list = cursor.ToList();
                pagingInfo.ActualLoadedItemCount = list.Count;
                return list;
            }
            return cursor;
        }

        protected  IMongoQuery GetFilterQuery(TFilter filter)
        {
            return BuildFilterQuery(filter).Any() ? Query.And(BuildFilterQuery(filter).ToArray()) : Query.Null;
        }

        protected virtual IMongoSortBy BuildSortExpression(TFilter filter)
        {
            IMongoSortBy sort =  SortBy.Null;
            if (filter.Ordering.Any())
            {
                var builder = new SortByBuilder();
                foreach (var order in filter.Ordering) 
                {
                    builder = order.Desc ? builder.Descending(order.Key) : builder.Ascending(order.Key);
                }
                sort = builder;
            }
            return sort;
        }

        protected abstract IEnumerable<IMongoQuery> BuildFilterQuery(TFilter filter);
    }
}