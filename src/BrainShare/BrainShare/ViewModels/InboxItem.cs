using System;
using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class InboxItem
    {
        public string Created { get; set; }
        public UserItemViewModel User { get; set; }
        public BookViewModel Book { get; set; }

        public InboxItem(DateTime created, Book book, User user)
        {
            Created = created.ToShortDateString();
            Book = new BookViewModel(book);
            User = new UserItemViewModel(user);
        }
    }
}