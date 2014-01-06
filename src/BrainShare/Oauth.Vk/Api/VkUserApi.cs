using System;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oauth.Vk.IApi;
using Oauth.Vk.Infrastructure;

namespace Oauth.Vk.Api
{
    public class VkUserApi : BaseVkApi,IVkUserApi
    {
        public VkUserApi(string accessToken) : base(accessToken)
        {
        }

        public T IsAppUser<T>()
        {
            throw new NotImplementedException();
        }

        public T Users_Get<T>(params string[]uids)
        {
            var parametrs = new NameValueCollection {{"uids", string.Join(",", uids)}};
            var json = Call("users.get", parametrs, AccessToken);

            json = JObject.Parse(json).SelectToken("response").ToString();

            return  JsonConvert.DeserializeObject<T>(json);  
        }

        public T Users_Search<T>()
        {
            throw new NotImplementedException();
        }

        public T GetUserSettings<T>()
        {
            throw new NotImplementedException();
        }

        public T LikesGetList<T>()
        {
            throw new NotImplementedException();
        }
    }
}
