using Newtonsoft.Json;

namespace Oauth.Vk.Dto.Wall
{
    [JsonObject]
    class VkPost
    {
        [JsonProperty("post_id")]
        public string postId { get; set; }
    }
}
