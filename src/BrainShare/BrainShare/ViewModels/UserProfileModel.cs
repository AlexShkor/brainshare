using System.Collections.Generic;
using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMe { get; set; }
        public SortedSet<string> Books { get; set; }
        public SortedSet<string> WishList { get; set; }

        public UserProfileModel(User user, string myId)
        {
            Id = user.Id;
            Name = user.FullName;
            IsMe = user.Id == myId;
            Books = user.Books;
            WishList = user.WishList;
        }

    }
}