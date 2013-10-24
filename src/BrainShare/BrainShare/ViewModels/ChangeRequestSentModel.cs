using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class ChangeRequestSentModel
    {
        public BookViewModel Book { get; set; }

        public UserItemViewModel Owner { get; set; }

        public ChangeRequestSentModel(Book book, User user)
        {
            Book = new BookViewModel(book);
            Owner = new UserItemViewModel(book);
        }
    }
}