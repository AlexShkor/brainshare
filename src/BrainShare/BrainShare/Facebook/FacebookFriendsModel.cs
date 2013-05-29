using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainShare.Facebook
{
    public class FacebookFriendsModel
    {
        public List<FacebookFriend> FriendsListing { get; set; }
    }

    public class FacebookFriend
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

}