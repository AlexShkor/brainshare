﻿using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class User
    {
        [BsonId]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FacebookId { get; set; }

        public List<string> Books { get; set; }
        public List<string> WishList { get; set; }

        public List<ChangeRequest> ChangeRequests { get; set; }

        public User()
        {
            Books = new List<string>();
            WishList = new List<string>();
            ChangeRequests = new List<ChangeRequest>();
        }
    }

    public class ChangeRequest
    {
        public string UserId { get; set; }
        public string BookId { get; set; }
    }
}