using MongoDB.Bson.Serialization.Attributes;

namespace Brainshare.Infrastructure.Documents
{
    public class CloudinaryImage
    {
        [BsonId]
        public string Id { get; set; }
        public string ImageId { get; set; }
        public string ImageUrl { get; set; }
    }
}