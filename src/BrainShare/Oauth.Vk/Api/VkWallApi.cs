using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oauth.Vk.IApi;
using Oauth.Vk.Infrastructure;
using Oauth.Vk.Infrastructure.Enums;

namespace Oauth.Vk.Api
{
    public class VkWallApi : BaseVkApi, IVkWallApi
    {
        public VkWallApi(string accessToken) : base(accessToken)
        {
        }

        public T Wall_Get<T>()
        {
            throw new NotImplementedException();
        }

        public T Wall_GetComments<T>()
        {
            throw new NotImplementedException();
        }

        public T Wall_GetById<T>()
        {
            throw new NotImplementedException();
        }

        public T Wall_Post<T>(string ownerId, string message, VkAttachment[] attachments, StatusName fromGroup, GroupPostSign signed)
        {
            var parametrs = new NameValueCollection { { "owner_id", ownerId }, { "message", message }, { "from_group", ((int)fromGroup).ToString() }, { "signed", ((int)signed).ToString() } };
            var json = Call("wall.post", parametrs);

            json = JObject.Parse(json).SelectToken("response").ToString();

            return JsonConvert.DeserializeObject<T>(json);  
        }

        public T Wall_GetLikes<T>()
        {
            throw new NotImplementedException();
        }
    }
}
