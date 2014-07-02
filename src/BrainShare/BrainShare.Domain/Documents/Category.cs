using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Domain.Documents
{
    public class Category
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}