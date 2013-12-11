﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class News
    {
        public News()
        {
        }

        public News(string message)
        {
            Message = message;
            Created = DateTime.UtcNow;
            Id = ObjectId.GenerateNewId(DateTime.Now).ToString();
        }

        [BsonId]
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}