using System;
using MongoDB.Driver;
using MvcMusicStore.Documents;

namespace MvcMusicStore.MongoDB
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
        
        public MongoCollection<AlbumDocument> Albums { get { return Database.GetCollection<AlbumDocument>("albums"); } }
        public MongoCollection<CartDocument> Carts { get { return Database.GetCollection<CartDocument>("carts"); } }
        public MongoCollection<OrderDocument> Orders { get { return Database.GetCollection<OrderDocument>("orders"); } }
    }
}
