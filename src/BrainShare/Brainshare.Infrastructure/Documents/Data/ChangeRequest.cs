using System;
using BrainShare.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Infrastructure.Documents.Data
{
    [BsonIgnoreExtraElements]
    public class ChangeRequest
    {
        public UserData User { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public DateTime Created { get; set; }
        public bool Viewed { get; set; }

        public ChangeRequest()
        {
            Created = DateTime.Now;
        }
    }
}