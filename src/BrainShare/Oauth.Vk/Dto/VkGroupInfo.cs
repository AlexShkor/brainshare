using Newtonsoft.Json;

namespace Brainshare.Vk.Dto
{
    public class VkGroupInfo
    {
        [JsonProperty("gid")]
        public string Gid { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("photo")]
        public string Photo { get; set; }
        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }
    }
}