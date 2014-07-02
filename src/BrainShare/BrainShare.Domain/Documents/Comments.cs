using System;
using System.Collections.Generic;
using BrainShare.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Domain.Documents
{
    public class CommentsDocument
    {
        [BsonId]
        public string Id { get; set; }

        public List<Comment> Comments { get; set; }

        public CommentsDocument()
        {
            Comments = new List<Comment>();
        }
    }

    public class Comment
    {
        public string Id { get; set; }

        public DateTime Timespan { get; set; }

        public string Content { get; set; }

        public UserData User { get; set; }

        public List<Comment> Replies { get; set; }

        public Comment()
        {
            Replies = new List<Comment>();
        }
    }
}