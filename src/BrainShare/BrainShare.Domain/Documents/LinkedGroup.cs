using System;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Domain.Documents
{
    public class LinkedGroup
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string GroupId { get; set; }

        public string AccessToken { get; set; }

        public bool Faild { get; set; }

        public string OwnerId { get; set; }
    }
}