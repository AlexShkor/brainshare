using System.Collections.Generic;
using BrainShare.Domain.Documents;
using BrainShare.Infrastructure.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BrainShare.Infrastructure.Services
{
    public class ActivityFeedsService : DocumentsService<ActivityFeed>
    {
        public ActivityFeedsService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<ActivityFeed> Items
        {
            get { return Database.ActivityFeeds; }
        }

        public IEnumerable<ActivityFeed> GetLast100()
        {
            return GetLast(100);
        }

        public IEnumerable<ActivityFeed> GetLast(int count)
        {
            return Items.FindAll().SetSortOrder(SortBy<ActivityFeed>.Descending(x => x.Created)).SetLimit(count);
        }
    }
}