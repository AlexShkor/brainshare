using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class CloudinaryImage
    {
        [BsonId]
        public string Id { get; set; }
        public string ImageId { get; set; }
        public string ImageUrl { get; set; }
    }
}