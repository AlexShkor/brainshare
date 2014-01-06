using System.Collections.Specialized;
using System.Text;

namespace Oauth.Vk.Infrastructure
{
    public abstract class BaseVkApi
    {
        private const string BaseApiCallurl = "https://api.vk.com/method/";
        protected string AccessToken;

        protected BaseVkApi(string accessToken)
        {
            AccessToken = accessToken;
        }

        protected string Call(string methodName, NameValueCollection parametrs, string accessToken, string method = "POST")
        {
            var postData = new StringBuilder();

            foreach (var key in parametrs.AllKeys)
            {
                postData.Append(key + "=" + parametrs[key] + "&");
            }
            // remove last character
            if (postData.Length != 0)
            {
                postData.Remove(postData.Length - 1, 1);
            }

            var url = BaseApiCallurl + methodName;

            var request = new MyWebRequest(url,method,postData.ToString());
            return request.GetResponse();
        }
    }
}
