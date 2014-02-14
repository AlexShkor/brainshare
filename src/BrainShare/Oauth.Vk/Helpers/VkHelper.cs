using Oauth.Vk.Infrastructure;

namespace Oauth.Vk.Helpers
{
    public class VkHelper
    {
        private const string BaseAuthorizeUrl = "https://oauth.vk.com/authorize?";
        private const string BaseAccessTokenUrl = "https://oauth.vk.com/access_token";
        public const string MobileRedirectUrl = "http://oauth.vk.com/blank.html";

        public const string MobileScope = "friends,photos,wall";
        public const string Scope = "friends,photos";

        public static string BuildAuthorizeUrl(string appId, string scope,string redirectUri, string responceType, string state,string apiVersion="5.5")
        {
            return string.Format("{0}client_id={1}&scope={2}&redirect_uri={3}&response_type={4}&state={5}&v={6}", BaseAuthorizeUrl, appId, scope, redirectUri, responceType,state, apiVersion);
        }

        public static string BuildAuthorizeUrlForMobile(string appId, string scope, string display = "page")
        {
            return string.Format("{0}client_id={1}&scope={2}&redirect_uri={3}&display={4}&response_type=token", BaseAuthorizeUrl, appId, scope, MobileRedirectUrl, display);
        }

        public static string GetAccessToken(string appId, string appSecret, string redirectUri, string code)
        {
            var postData = string.Format("client_id={0}&client_secret={1}&code={2}&redirect_uri={3}", appId, appSecret, code, redirectUri);

            var myRequest = new MyWebRequest(BaseAccessTokenUrl, "POST", postData);

            return myRequest.GetResponse();
        }
    }
}
