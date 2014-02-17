using Newtonsoft.Json;

namespace Brainshare.Vk.Dto
{
    [JsonObject]
    public class VkPost
    {
        [JsonProperty("post_id")]
        public string PostId { get; set; }
    }
}
