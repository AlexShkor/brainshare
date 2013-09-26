using System;
using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class MessageViewModel
    {
        public string UserId { get; set; }

        public string From { get; set; }

        public string Content { get; set; }

        public string Posted { get; set; }

        public string Class { get; set; }

        public MessageViewModel()
        {
            
        }

        public MessageViewModel(Message message, User recipient)
        {
            var notMe = message.UserId == recipient.Id;
            Init(message.UserId, message.Content, message.Posted.ToString("o"), notMe, recipient.FullName);
        }

        public void Init(string userId, string content, string posted, bool notMe, string from = null)
        {
            UserId = userId;
            From = notMe ? from : "меня";
            Class = notMe ? "span6 alert alert-info" : "span6 alert pull-right alert-success text-right";
            Posted = posted;
            Content = content;
        }
    }
}