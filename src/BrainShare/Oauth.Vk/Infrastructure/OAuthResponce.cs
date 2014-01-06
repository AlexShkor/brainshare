using Newtonsoft.Json;

namespace Brainshare.Infrastructure.VK
{
    [JsonObject]
    public class OAuthResponce
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
