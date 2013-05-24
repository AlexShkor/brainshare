using System;
using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class MessageViewModel
    {
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
            Init(message.Content, message.Posted, notMe, recipient.FullName);
        }

        public void  Init(string content, DateTime posted, bool notMe, string from = null)
        {
            From = notMe ? from : "Я";
            Class = notMe ? "span6 alert alert-info" : "span6 alert pull-right alert-success text-right";
            Posted = posted.ToRelativeDate();
            Content = content;
        }
    }
}