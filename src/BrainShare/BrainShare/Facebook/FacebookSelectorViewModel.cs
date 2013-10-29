using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainShare.Facebook
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