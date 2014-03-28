using System;
using MongoDB.Driver;

namespace Brainshare.Infrastructure.Databases
{
    public class MongoAdminDatabase
    {
        /// <summary>
        /// MongoDB Server
        /// </summary>
        private readonly MongoServer _server;

        /// <summary>
        /// Name of database 
        /// </summary>
        private readonly string _databaseName;

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoAdminDatabase(String connectionString)
        {
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var mongoClient = new MongoClient(connectionString);
            _server = mongoClient.GetServer();
        }

        /// <summary>
        /// MongoDB Server
        /// </summary>
        public MongoServer Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Get database
        /// </summary>
        public MongoDatabase Database
        {
            get { return _server.GetDatabase(_databaseName); }
        }

        public MongoAdminDatabase EnsureIndexes()
        {
            return this;
        }
    }
}
