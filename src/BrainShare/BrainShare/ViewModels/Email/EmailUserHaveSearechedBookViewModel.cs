using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class EmailUserHaveSearechedBookViewModel : BaseEmailViewModel
    {
        public User Owner { get; set; }
        public BookViewModel Book { get; set; }

        public string OwnerProfileLink
        {
            get
            {
               return BaseAddress + "/profile/view/" + Owner.Id;
            }      
        }
    }
}