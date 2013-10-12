using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver;

namespace BrainShare.Services
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