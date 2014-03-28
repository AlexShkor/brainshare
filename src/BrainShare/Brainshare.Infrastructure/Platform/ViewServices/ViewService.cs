using System.Collections.Generic;
using Brainshare.Infrastructure.Databases;
using Brainshare.Infrastructure.Platform.Mongo;

namespace Brainshare.Infrastructure.Platform.ViewServices
{
    public abstract class ViewService<T>
    {
        protected readonly MongoViewDatabase Database;

        protected ViewService(MongoViewDatabase database)
        {
            Database = database;
        }

        protected abstract ReadonlyMongoCollection<T> Items { get; }

        public virtual T GetById(string id)
        {
            return Items.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Items.FindAll();
        }
    }
}
