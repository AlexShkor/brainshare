using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class RequestViewModel
    {
        public User CurrentUser { get; set; }
        public User RequestedUser { get; set; }
        public Book Book { get; set; }
    }
}