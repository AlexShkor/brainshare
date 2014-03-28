using System.Collections.Generic;
using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Documents.Data;
using Brainshare.Infrastructure.Mongo;
using MongoDB.Driver.Builders;

namespace Brainshare.Infrastructure.Services
{
    public class ShellUserService:DocumentsService<ShellUser>
    {
        public ShellUserService(MongoDocumentsDatabase database) : base(database)
        {
        }

        public ShellUser GetUserByLoginServiceInfo(LoginServiceTypeEnum loginServiceType, string serviceId)
        {
            switch (loginServiceType)
            {
                case LoginServiceTypeEnum.Facebook:
                    return GetUserByFbId(serviceId);
                case LoginServiceTypeEnum.Vk:
                    return GetUserByVkId(serviceId);
                case LoginServiceTypeEnum.Email:
                    return GetUserByEmail(serviceId);
            }
            return null;
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

        public ShellUser GetUserByVkId(string id)
        {
            return Items.FindOne(Query<ShellUser>.EQ(x => x.VkId, id));
        }

        public ShellUser GetUserByFbId(string id)
        {
            return Items.FindOne(Query<ShellUser>.EQ(x => x.FacebookId, id));
        }

        protected override MongoDB.Driver.MongoCollection<ShellUser> Items
        {
            get { return Database.ShellUsers; }
        }
    }
}