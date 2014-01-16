using System.Collections.Generic;

namespace BrainShare.ViewModels
{
    public class SearchByGoogleViewModel
    {
        public IEnumerable<string> MyBooksIds { get; set; }
        public IEnumerable<string> WishBooksIds { get; set; }
    }
}