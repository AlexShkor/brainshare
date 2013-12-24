using BrainShare.Documents;
using BrainShare.Infrastructure.Mongo;
using MongoDB.Driver.Builders;

namespace Brainshare.Infrastructure.Services
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

        public ShellUser GetUserByEmail(string email)
        {
            return Items.FindOne(Query<ShellUser>.EQ(x => x.Email, email));
        }

        protected override MongoDB.Driver.MongoCollection<ShellUser> Items
        {
            get { return Database.ShellUsers; }
        }
    }
}