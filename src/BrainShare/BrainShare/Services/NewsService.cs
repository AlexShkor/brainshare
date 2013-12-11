using System;
using System.Collections.Generic;
using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BrainShare.Services
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