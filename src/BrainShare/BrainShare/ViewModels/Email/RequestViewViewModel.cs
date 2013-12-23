using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class EmailRequestViewModel : BaseEmailViewModel
    {
        public User CurrentUser { get; set; }
        public User RequestedUser { get; set; }
        public Book Book { get; set; }

        public string RequestedUserProfileLink
        {
            get { return BaseAddress + "/profile/view/" + RequestedUser.Id; }
        }

        public string RequestedBookLink
        {
            get { return BaseAddress + "/books/info/" + Book.Id; }
        }
    }
}