using System;
using Brainshare.Infrastructure.Documents;
using Uniform;

namespace Brainshare.Infrastructure.Databases
{
    public static class ViewDatabases
    {
        public const string Mongodb = "mongodb";
        public const string Sql = "sql";
    }

    public static class ViewCollections
    {
        public const string Users = "users";
    }

    public class ViewDatabase
    {
        #region ViewDatabase
        private readonly UniformDatabase _db;

        public ViewDatabase(UniformDatabase db)
        {
            _db = db;
        }

        /// <summary>
        /// Helper method for mongodb collections
        /// </summary>
        private IDocumentCollection<TDocument> GetMongoCollection<TDocument>(String collectionName) where TDocument : new()
        {
            return _db.GetCollection<TDocument>(ViewDatabases.Mongodb, collectionName);
        }

        /// <summary>
        /// Helper method for sql collections (tables)
        /// </summary>
        private IDocumentCollection<TDocument> GetSqlCollection<TDocument>(String tableName) where TDocument : new()
        {
            return _db.GetCollection<TDocument>(ViewDatabases.Sql, tableName);
        }

        #endregion

        

        public IDocumentCollection<User> Users
        {
            get { return GetMongoCollection<User>(ViewCollections.Users); }
        }
    }
}