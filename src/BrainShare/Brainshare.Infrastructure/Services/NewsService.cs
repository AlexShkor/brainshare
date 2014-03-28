using System.Collections.Generic;
using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Brainshare.Infrastructure.Services
{
    public class NewsService : DocumentsService<News>
    {
        public NewsService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<News> Items
        {
            get { return Database.News; }
        }

        public IEnumerable<News> GetByIds(IEnumerable<string> ids)
        {
            return Items.Find(Query<News>.In(x => x.Id, ids));
        }
    }
}