using System;
using MongoDB.Driver;

namespace Brainshare.Infrastructure.Databases
{
    public class MongoInstance
    {
        private readonly MongoServer _server;
        private readonly string _defaultDatabase;

        public MongoUrl MongoUrl { get; private set; }

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoInstance(String connectionString, bool authenticateToAdmin)
        {
            var builder = new MongoUrlBuilder(connectionString);
            _defaultDatabase = builder.DatabaseName;
            if (authenticateToAdmin)
                builder.DatabaseName = "admin";
            MongoUrl = builder.ToMongoUrl();
            _server = new MongoClient(MongoUrl).GetServer();
        }

        public string DefaultDatabase
        {
            get { return _defaultDatabase; }
        }

        /// <summary>
        /// MongoDB Server
        /// </summary>
        public MongoServer Server
        {
            get
            {
                return _server;
            }
        }
    }
}