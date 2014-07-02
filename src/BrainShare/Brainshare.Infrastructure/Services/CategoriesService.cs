using System.Collections.Generic;
using System.Linq;
using BrainShare.Domain.Documents;
using BrainShare.Infrastructure.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Brainshare.Infrastructure.Services
{
    public class CategoriesService: DocumentsService<Category>
    {
        public CategoriesService(MongoDocumentsDatabase database)
            : base(database)
        {
        }

        protected override MongoCollection<Category> Items
        {
            get { return Database.Categories; }
        }

        public Category Find(string name)
        {
            return Items.AsQueryable().FirstOrDefault(x => x.Name == name);
        }

        public new IEnumerable<Category> GetAll()
        {
            return base.GetAll().OrderBy(x => x.Name);
        }
    }
}