using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents;
using BrainShare.Documents.Data;

namespace BrainShare.ViewModels
{
    public class FollowersViewModel
    {
        public FollowersViewModel(User user)
        {
            Publishers = user.Publishers;
        }

        public List<Publisher> Publishers { get; set; }

        public bool FriendsExist{
            get { return Publishers != null && Publishers.Any(); }
        }
    }
}