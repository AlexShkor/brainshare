using System.Collections.Generic;
using BrainShare.Controllers;

namespace BrainShare.ViewModels
{
    public class ConsiderRequestViewModel
    {
        public BookViewModel YourBook { get; set; }
        public string FromUserId { get; set; }
        public List<BookViewModel> AllBooks { get; set; }
        public UserItemViewModel FromUser { get; set; }
        public List<string> BooksYouNeedTitles { get; set; }
    }
}