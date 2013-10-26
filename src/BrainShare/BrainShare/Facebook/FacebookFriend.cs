using Newtonsoft.Json;

namespace BrainShare.Facebook
{
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