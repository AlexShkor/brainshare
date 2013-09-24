using System.Collections.Generic;

namespace BrainShare.Controllers
{
    public class AcceptRequestViewModel
    {
        public BookViewModel YourBook { get; set; }
        public string FromUserId { get; set; }
        public List<BookViewModel> AllBooks { get; set; }
        public UserItemViewModel FromUser { get; set; }
        public List<string> BooksYouNeedTitles { get; set; }
    }
}