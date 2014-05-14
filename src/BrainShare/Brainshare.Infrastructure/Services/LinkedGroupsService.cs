using System.Collections.Generic;
using BrainShare.Domain.Documents;
using BrainShare.Infrastructure.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Brainshare.Infrastructure.Services
{
    public class LinkedGroupsService : DocumentsService<LinkedGroup>
    {
        public LinkedGroupsService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<LinkedGroup> Items
        {
            get { return Database.LinkedGroups; }
        }

        public IEnumerable<LinkedGroup> GetForUser(string userId)
        {
            return Items.Find(Query<LinkedGroup>.EQ(x => x.OwnerId, userId));
        }

        public IEnumerable<LinkedGroup> GetAllAuthorized()
        {
            return Items.Find(Query<LinkedGroup>.NE(x => x.AccessToken, null));
        }

        public void SetFaild(string id)
        {
            Items.Update(Query.EQ("_id", id), Update<LinkedGroup>.Set(x=> x.Faild, true));
        }
    }
}