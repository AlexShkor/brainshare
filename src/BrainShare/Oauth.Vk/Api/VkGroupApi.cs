using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oauth.Vk.Infrastructure;

namespace Oauth.Vk.Api
{
    public class VkGroupApi: BaseVkApi
    {
        public VkGroupApi(string accessToken) : base(accessToken)
        {
        }

        public VkGroupInfo GetInfo(string id)
        {
            var parametrs = new NameValueCollection { { "gids", id } };
            var json = Call("groups.getById", parametrs);

            json = JObject.Parse(json).SelectToken("response").ToString();

            return JsonConvert.DeserializeObject<List<VkGroupInfo>>(json)[0];  
        }
    }

    public class VkGroupInfo
    {
        public string gid { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string photo { get; set; }
        public bool is_admin { get; set; }
    }
}