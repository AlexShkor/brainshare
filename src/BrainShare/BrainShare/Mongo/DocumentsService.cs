using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BrainShare.Mongo
{
    public abstract class DocumentsService<T>
    {
        protected readonly MongoDocumentsDatabase Database;

        protected DocumentsService(MongoDocumentsDatabase database)
        {
            Database = database;
        }

        protected abstract MongoCollection<T> Items { get; }

        public T GetById(string id)
        {
            return Items.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Items.FindAll();
        }

        public void Save(T item)
        {
            Items.Save(item);
        }

        public void Insert(T item)
        {
            Items.Insert(item);
        }

        public void InsertBatch(params T[] items)
        {
            Items.InsertBatch(items);
        }

        protected static string GenerateId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
