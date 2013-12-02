using System.Collections.Generic;
using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver.Builders;

namespace BrainShare.Services
{
    public class PublicShellService:DocumentsService<PublicShell>
    {
        public PublicShellService(MongoDocumentsDatabase database) : base(database)
        {
        }

        public IEnumerable<PublicShell> GetUserShells(string userId)
        {
            return Items.Find(Query<PublicShell>.EQ(s => s.CreatorId, userId));
        }

        protected override MongoDB.Driver.MongoCollection<PublicShell> Items
        {
            get { return Database.PublicShell; }
        }
    }
}