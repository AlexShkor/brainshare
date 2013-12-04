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

        public ShellUser GetByCredentials(string email, string password)
        {
            return
                Items.FindOne(Query.And(Query<ShellUser>.EQ(x => x.Email, email),
                                        Query<ShellUser>.EQ(x => x.Password, password)));
        }

        protected override MongoDB.Driver.MongoCollection<ShellUser> Items
        {
            get { return Database.ShellUsers; }
        }
    }
}