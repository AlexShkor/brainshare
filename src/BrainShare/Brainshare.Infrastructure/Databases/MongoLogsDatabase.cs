using System;
using MongoDB.Driver;

namespace Brainshare.Infrastructure.Databases
{
    public class MongoLogsDatabase
    {
        private readonly MongoInstance _mongo;

        public MongoLogsDatabase(String connectionString, bool authenticateToAdmin)
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

        public MongoLogsDatabase EnsureIndexes()
        {
            //Logs.EnsureIndex(IndexKeys.Descending("Command.Metadata.CreatedDate"));
            //Logs.EnsureIndex(IndexKeys.Descending("Command.Metadata.CreatedDate").Ascending("Errors"));
            return this;
        }

        public MongoCollection Logs
        {
            get { return Database.GetCollection("logs"); }
        }

        public MongoCollection AppLogs
        {
            get { return Database.GetCollection("app_logs"); }
        }
    }
}
