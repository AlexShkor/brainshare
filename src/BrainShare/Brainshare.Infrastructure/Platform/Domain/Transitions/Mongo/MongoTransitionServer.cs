using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Brainshare.Infrastructure.Platform.Domain.Transitions.Mongo
{
    public class MongoTransitionServer
    {
        private readonly MongoServer _server;

        private readonly string _databaseName;

        /// <summary>
        /// Collection for storing commits data
        /// </summary>
        private const string TransitionsCollectionName = "transitions";
        private const string SnapshotsCollectionName = "snapshots";

        private readonly MongoCollectionSettings<BsonDocument> _transitionSettings;
        private readonly MongoCollectionSettings<BsonDocument> _snapshotSettings;

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoTransitionServer(String connectionString, String transitionsCollectionName = null)
        {
            transitionsCollectionName = transitionsCollectionName ?? TransitionsCollectionName;

            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var client = new MongoClient(connectionString);
            _server = client.GetServer();

            _transitionSettings = Database.CreateCollectionSettings<BsonDocument>(transitionsCollectionName);
            _transitionSettings.WriteConcern = WriteConcern.Acknowledged;
            _transitionSettings.AssignIdOnInsert = false;

            _snapshotSettings = Database.CreateCollectionSettings<BsonDocument>(SnapshotsCollectionName);
            _snapshotSettings.WriteConcern = WriteConcern.Acknowledged;
            _snapshotSettings.AssignIdOnInsert = false;
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

        /// <summary>
        /// Get commits collection
        /// </summary>
        public MongoCollection<BsonDocument> Transitions
        {
            get { return Database.GetCollection(_transitionSettings); }
        }

        public MongoCollection<BsonDocument> Snapshots
        {
            get { return Database.GetCollection(_snapshotSettings); }
        }
    }
}