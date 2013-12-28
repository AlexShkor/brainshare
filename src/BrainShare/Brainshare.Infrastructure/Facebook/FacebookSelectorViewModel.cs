using System.Collections.Generic;

namespace Brainshare.Infrastructure.Facebook
{
    public class FacebookSelectorViewModel
    {
        public FacebookSelectorViewModel(List<FacebookFriend> friends)
        {
            FacebookFriends = friends;
        }

        public List<FacebookFriend> FacebookFriends { get; set; }
    }
}