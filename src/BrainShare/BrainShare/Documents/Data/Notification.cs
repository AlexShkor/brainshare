using System;
using MongoDB.Bson;

namespace BrainShare.Documents
{
    public class Notification
    {
        public Notification()
        {
        }

        public Notification(string message)
        {
            Message = message;
            Created = DateTime.UtcNow;
            WasRead = false;
            Id = ObjectId.GenerateNewId(DateTime.Now).ToString();
        }

        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public bool WasRead { get; set; }
    }
}