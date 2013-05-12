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

        public MessageViewModel(Message message, User recipient)
        {
            var notMe = message.UserId == recipient.Id;
            From = notMe ? recipient.FullName : "Я";
            Class = notMe ? "span6 alert alert-info" : "span6 alert pull-right alert-success text-right";
            Posted = message.Posted.ToRelativeDate();
            Content = message.Content;
        }
    }
}