using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class Thread
    {
        [BsonId]
        public string Id { get; set; }

        public string OwnerId { get; set; }
        public string OwnerName { get; set; }

        public List<string> Users { get; set; }

        public string RecipientId { get; set; }
        public string RecipientName { get; set; }

        public List<Message> Messages { get; set; }

        public Thread()
        {
            Messages = new List<Message>();
            Users = new List<string>();
        }

        public Thread(string userId, string userName, string recipientUserId, string recipientName)
            : this()
        {
            Id = ObjectId.GenerateNewId().ToString();
            OwnerId = userId;
            OwnerName = userName;
            RecipientName = recipientName;
            RecipientId = recipientUserId;
            Users.Add(userId);
            Users.Add(recipientUserId);
        }

        public bool ContainsUser(string userId)
        {
            return Users.Contains(userId);
        }

        public string GetSecondUserName(string me)
        {
            return OwnerId == me ? RecipientName : OwnerName;
        }

        public string GetSecondUserId(string me)
        {
            return OwnerId == me ? RecipientId : OwnerId;
        }
    }
}