using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver;

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
    }
}