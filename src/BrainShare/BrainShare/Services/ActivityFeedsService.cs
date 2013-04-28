using System.Collections.Generic;
using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BrainShare.Services
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
            return Items.FindAll().SetSortOrder(SortBy<ActivityFeed>.Descending(x => x.Created)).SetLimit(100);
        }
    }
}