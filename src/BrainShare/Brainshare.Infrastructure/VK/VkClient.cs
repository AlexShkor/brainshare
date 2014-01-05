namespace Brainshare.Infrastructure.VK
{
    public class VkClient
    {
        private const string VkAuthorizeUrl = "https://oauth.vk.com/authorize";
        private const string ApiVersion = "";

        public static string GetLoginUrl(string appId, string appSecret, string callbackUri, string scope, string responceType = "code")
        {
            return string.Format("{0}?client_id={1}&scope={2}&redirect_uri={3}&responce_type={4}&v={5}",VkAuthorizeUrl,appId,scope,callbackUri,responceType,ApiVersion);
        }
    }
}
