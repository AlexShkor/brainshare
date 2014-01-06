namespace Brainshare.Infrastructure.VK
{
    public class VkHelper
    {
        private const string BaseAuthorizeUrl = "https://oauth.vk.com/authorize?";

        public static string BuildAuthorizeUrl(string appId, string scope,string redirectUri, string responceType,string apiVersion="5.5")
        {
            return string.Format("{0}client_id={1}&scope={2}&redirect_uri={3}&response_type={4}&v={5}", BaseAuthorizeUrl, appId, scope, redirectUri, responceType, apiVersion);
        }
    }
}
