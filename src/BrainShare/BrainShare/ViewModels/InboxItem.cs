using System;
using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Documents.Data;

namespace BrainShare.ViewModels
{
    public class InboxItem
    {
        public string Created { get; set; }
        public UserItemViewModel User { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }

        public bool IsNew { get; set; }

        public InboxItem(DateTime created, Book book, User user, bool isNew)
        {
            Created = created.ToShortDateString();
            BookId = book.Id;
            BookTitle = book.Title;
            User = new UserItemViewModel(user);
            IsNew = isNew;
        }

        public InboxItem(ChangeRequest request)
        {
            Created = request.Created.ToShortDateString();
            User = new UserItemViewModel(request.User);
            IsNew = !request.Viewed;
            BookId = request.BookId;
            BookTitle = request.BookTitle;
        }
    }
}