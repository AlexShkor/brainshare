using BrainShare.Documents;
using Brainshare.Infrastructure.Documents;

namespace BrainShare.ViewModels
{
    public class ThreadItemViewModel
    {

        public string ThreadId { get; set; }

        public string To { get; set; }

        public string LinkToProfile { get; set; }

        public bool IsNew { get; set; }

        public ThreadItemViewModel(Thread thread, string me, User user)
        {
            ThreadId = thread.Id;
            IsNew = user.ThreadsWithUnreadMessages.Contains(thread.Id);
            To = thread.GetSecondUserName(me);
            LinkToProfile = string.Format("/profile/view/{0}", thread.GetSecondUserId(me));
        }

    }
}