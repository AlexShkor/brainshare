using System.Collections.Generic;

namespace BrainShare.Controllers
{
    public class AcceptRequestViewModel
    {
        public BookViewModel YourBook { get; set; }
        public string FromUserId { get; set; }
        public List<BookViewModel> AllBooks { get; set; }
        public List<BookViewModel> BooksYouNeed { get; set; }

        public UserItemViewModel FromUser { get; set; }
    }
}