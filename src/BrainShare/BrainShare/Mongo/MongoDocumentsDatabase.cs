using System;
using BrainShare.Documents;
using MongoDB.Driver;

namespace BrainShare.Mongo
{
    public class MongoDocumentsDatabase
    {
        /// <summary>
        /// MongoDB Server
        /// </summary>
        private readonly MongoServer _server;

        /// <summary>
        /// Name of database 
        /// </summary>
        private readonly string _databaseName;

        public MongoUrl MongoUrl { get; private set; }

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoDocumentsDatabase(String connectionString)
        {
            MongoUrl = MongoUrl.Create(connectionString);
            _databaseName = MongoUrl.DatabaseName;
            _server = new MongoClient(connectionString).GetServer();
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

        public MongoCollection<User> Users { get { return Database.GetCollection<User>("users"); } }
        public MongoCollection<Book> Books { get { return Database.GetCollection<Book>("books"); } }
        public MongoCollection<ActivityFeed> ActivityFeeds { get { return Database.GetCollection<ActivityFeed>("feeds"); } }
        public MongoCollection<Thread> Threads { get { return Database.GetCollection<Thread>("threads"); } }
    }
}