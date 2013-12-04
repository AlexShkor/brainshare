using System.Collections.Generic;
using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver.Builders;

namespace BrainShare.Services
{
    public class ShellUserService:DocumentsService<ShellUser>
    {
        public ShellUserService(MongoDocumentsDatabase database) : base(database)
        {
        }

        public IEnumerable<ShellUser> GetUserShells(string userId)
        {
            return null; //Items.Find(Query<PublicShell>.EQ(s => s.CreatorId, userId));
        }

        protected override MongoDB.Driver.MongoCollection<ShellUser> Items
        {
            get { return Database.ShellUsers; }
        }
    }
}