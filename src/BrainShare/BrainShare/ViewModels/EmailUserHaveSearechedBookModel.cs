using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class EmailUserHaveSearechedBookModel : BaseEmailModel
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