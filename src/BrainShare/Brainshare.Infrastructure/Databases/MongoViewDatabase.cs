using System;
using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Platform.Mongo;
using MongoDB.Driver;

namespace Brainshare.Infrastructure.Databases
{
    public class MongoViewDatabase
    {
        private readonly MongoInstance _mongo;

        public MongoViewDatabase(String connectionString, bool authenticateToAdmin)
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

        public MongoViewDatabase EnsureIndexes()
        {
            return this;
        }

        public ReadonlyMongoCollection GetCollection(String collectionName)
        {
            return Database.GetCollection(collectionName).ToReadonly();
        }

        protected ReadonlyMongoCollection<TDocument> GetCollection<TDocument>(String collectionName)
        {
            return Database.GetCollection<TDocument>(collectionName).ToReadonly();
        }


        public ReadonlyMongoCollection<User> Users
        {
            get { return GetCollection<User>(ViewCollections.Users); }
        }
    }
}
