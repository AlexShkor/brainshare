using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class MessagingThreadViewModel
    {
        public string ThreadId { get; set; }
        public string UserId { get; set; }
        public string RecipientId { get; set; }
        public string RecipientName { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public MessagingThreadViewModel(Thread thread, User me, User recipient)
        {
            ThreadId = thread.Id;
            UserId = me.Id;
            RecipientId = recipient.Id;
            RecipientName = recipient.FullName;
            Messages = thread.Messages.OrderByDescending(x=> x.Posted).Select(x => new MessageViewModel(x, recipient)).ToList();
        }
    }
}