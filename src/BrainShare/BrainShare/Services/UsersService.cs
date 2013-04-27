using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver;

namespace BrainShare.Services
{
    public class UsersService: DocumentsService<User>
    {
        public UsersService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<User> Items
        {
            get { return Database.Users; }
        }
    }
}