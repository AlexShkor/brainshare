using Newtonsoft.Json;

namespace Oauth.Vk.Dto.VkUserApi
{
    [JsonObject]
    public class VkUser
    {
        [JsonProperty("uid")]
        public string UserId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("photo_200")]
        public string AvatarUrl { get; set; }
    }
}
