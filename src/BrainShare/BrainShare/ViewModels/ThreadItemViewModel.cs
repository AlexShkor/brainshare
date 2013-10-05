using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class ThreadItemViewModel
    {

        public string ThreadId { get; set; }

        public string To { get; set; }

        public bool IsNew { get; set; }

        public ThreadItemViewModel(Thread thread, string me, User user)
        {
            ThreadId = thread.Id;
            IsNew = user.ThreadsWithUnreadMessages.Contains(thread.Id);
            To = thread.GetSecondUserName(me);
        }

    }
}