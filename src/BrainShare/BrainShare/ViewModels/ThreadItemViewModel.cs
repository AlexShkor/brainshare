using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class ThreadItemViewModel
    {

        public string ThreadId { get; set; }

        public string To { get; set; }

        public ThreadItemViewModel(Thread thread, string me)
        {
            ThreadId = thread.Id;
            To = thread.GetSecondUserName(me);
        }

    }
}