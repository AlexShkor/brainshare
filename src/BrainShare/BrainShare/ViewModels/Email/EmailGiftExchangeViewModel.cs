using BrainShare.Documents;

namespace BrainShare.ViewModels.Email
{
    public class EmailGiftExchangeViewModel :BaseEmailModel
    {
        public Book Book { get; set; }
        public User Owner { get; set; }
        public User Receiver { get; set; }


        public string OwnerProfileLink
        {
            get { return BaseAddress + "/profile/view/" + Owner.Id; }
        }

        public string GiftedBookLink
        {
            get { return BaseAddress + "/books/info/" + Book.Id; }
        }
    }
}