using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Mongo;
using MongoDB.Driver;

namespace Brainshare.Infrastructure.Services
{
    public class CloudinaryImagesService : DocumentsService<CloudinaryImage>
    {
        public CloudinaryImagesService(MongoDocumentsDatabase database)
            : base(database)
        {
        }

        protected override MongoCollection<CloudinaryImage> Items
        {
            get { return Database.CloudinaryImages; }
        }
        
        public void AddImage(CloudinaryImage image)
        {
            Items.Save(image);
        }
    }
}