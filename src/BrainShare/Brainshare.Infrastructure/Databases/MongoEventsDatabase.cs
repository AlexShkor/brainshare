using System;
using Brainshare.Infrastructure.Platform.Upgrade;
using MongoDB.Driver;

namespace Brainshare.Infrastructure.Databases
{
    public class MongoEventsDatabase
    {
        private readonly MongoInstance _mongo;

        public MongoEventsDatabase(String connectionString, bool authenticateToAdmin)
        {
            _mongo = new MongoInstance(connectionString, authenticateToAdmin);
        }

        /// <summary>
        /// Get database
        /// </summary>
        public MongoDatabase Database
        {
            get { return _mongo.Server.GetDatabase(_mongo.DefaultDatabase); }
        }

        public MongoInstance MongoInstance
        {
            get { return _mongo; }
        }

        public MongoCollection Transitions
        {
            get { return Database.GetCollection("transitions"); }
        }

        public MongoCollection<UpgradeDocument> Upgrades
        {
            get { return Database.GetCollection<UpgradeDocument>("upgrades"); }
        }
    }
}
