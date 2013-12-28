using BrainShare.Domain.Documents;

namespace BrainShare.ViewModels
{
    public class ChangeRequestSentModel
    {
        public BookViewModel Book { get; set; }

        public UserItemViewModel Owner { get; set; }

        public ChangeRequestSentModel(Book book)
        {
            Book = new BookViewModel(book);
            Owner = new UserItemViewModel(book);
        }
    }
}