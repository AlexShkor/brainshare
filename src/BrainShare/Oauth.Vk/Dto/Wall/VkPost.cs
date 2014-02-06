using Newtonsoft.Json;

namespace Oauth.Vk.Dto.Wall
{
    [JsonObject]
    public class VkPost
    {
        [JsonProperty("post_id")]
        public string PostId { get; set; }
    }
}
