using System;

namespace BrainShare.Domain.Documents.Data
{
    public class Message
    {
        public string UserId { get; set; }

        public string Content { get; set; }

        public DateTime Posted { get; set; }

        public Message()
        {

        }

        public Message(string userId, string content)
        {
            UserId = userId;
            Content = content;
            Posted = DateTime.Now;
        }
    }
}