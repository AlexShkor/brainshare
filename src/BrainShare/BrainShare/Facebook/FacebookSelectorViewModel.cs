using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

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


    [JsonObject]
    public class FacebookFriend
    {
        [JsonProperty("uid")]
        public string FacebookId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("pic_square")]
        public string AvatarUrl { get; set; }

        public string Id { get; set; }
    }

}