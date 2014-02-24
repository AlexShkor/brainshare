using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Brainshare.Vk.Dto;
using Brainshare.Vk.Infrastructure;
using Brainshare.Vk.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Brainshare.Vk.Api
{
    public class VkApi
    {
        private const string BaseApiCallurl = "https://api.vk.com/method/";
        protected string AccessToken;

        public VkApi(string accessToken)
        {
            AccessToken = accessToken;
        }

        public List<VkUser> GetUsers(string[] fields, string[]uids)
        {
            var parametrs = new NameValueCollection { { "uids", string.Join(",", uids) }, { "fields", string.Join(",", fields) } };
            var json = Call("users.get", parametrs);

            return Parse<List<VkUser>>(json);  
        }

        public void Post(string ownerId, string message, string url, StatusName fromGroup, GroupPostSign signed)
        {
            var parametrs = new NameValueCollection
            {
                { "owner_id", ownerId }, 
                { "message", message }, 
                { "from_group", ((int)fromGroup).ToString() }, 
                { "signed", ((int)signed).ToString() },
                { "attachments" , url}
            };
            var json = Call("wall.post", parametrs);
            var result = Parse<VkPost>(json);
        }


        public VkCity GetCity(string id)
        {
            var parametrs = new NameValueCollection { { "cids", string.Join(",", new[] { id }) } };
            var json = Call("places.getCityById", parametrs);

            return Parse<List<VkCity>>(json).First();
        }

        public VkCountry GetCountry(string id)
        {
            var parametrs = new NameValueCollection { { "cids", string.Join(",", new[] { id }) } };
            var json = Call("places.getCountryById", parametrs);

            return Parse<List<VkCountry>>(json).First();
        }

        public VkGroupInfo GetGroupInfo(string id)
        {
            var parametrs = new NameValueCollection { { "gids", id } };
            var json = Call("groups.getById", parametrs);

            return Parse<List<VkGroupInfo>>(json)[0];
        }

        private T Parse<T>(string json)
        {
            var response = JObject.Parse(json);
            json = response.SelectToken("response").ToString();
            return JsonConvert.DeserializeObject<T>(json);
        }

        private string Call(string methodName, NameValueCollection parametrs, string method = "POST")
        {
            if (!string.IsNullOrEmpty(AccessToken))
            {
                parametrs.Add("access_token", AccessToken);
            }
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

            var request = new VkWebRequest(url, method, postData.ToString());
            return request.GetResponse();
        }
    }
}
