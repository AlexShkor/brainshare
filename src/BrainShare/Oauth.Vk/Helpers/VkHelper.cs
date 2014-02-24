using Brainshare.Vk.Infrastructure;

namespace Brainshare.Vk.Helpers
{
    public class VkAuth
    {
        private const string BaseAuthorizeUrl = "https://oauth.vk.com/authorize?";
        private const string BaseAccessTokenUrl = "https://oauth.vk.com/access_token";
        public const string MobileRedirectUrl = "http://oauth.vk.com/blank.html";

        public const string MobileScope = "photos,wall";
        public const string Scope = "photos";

        public static string BuildAuthorizeUrl(string appId, string redirectUri)
        {
            const string apiVersion = "5.10";
            const string responseType = "code";
            return string.Format("{0}client_id={1}&scope={2}&redirect_uri={3}&response_type={4}&v={5}",
                BaseAuthorizeUrl, appId, Scope, redirectUri, responseType, apiVersion);
        }

        public static string BuildAuthorizeUrlForMobile(string appId)
        {
            const string display = "page";
            return string.Format("{0}client_id={1}&scope={2}&redirect_uri={3}&display={4}&response_type=token", BaseAuthorizeUrl, appId, MobileScope, MobileRedirectUrl, display);
        }

        public static string GetAccessToken(string appId, string appSecret, string redirectUri, string code)
        {
            var postData = string.Format("client_id={0}&client_secret={1}&code={2}&redirect_uri={3}", appId, appSecret, code, redirectUri);

            var myRequest = new VkWebRequest(BaseAccessTokenUrl, "POST", postData);

            return myRequest.GetResponse();
        }
    }
}
