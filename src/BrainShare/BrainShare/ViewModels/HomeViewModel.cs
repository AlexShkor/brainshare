using System.Collections.Generic;
using Brainshare.Infrastructure.Documents;

namespace BrainShare.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ActivityFeed> Feeds { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
        public IEnumerable<BookViewModel> Books { get; set; }
    }
}