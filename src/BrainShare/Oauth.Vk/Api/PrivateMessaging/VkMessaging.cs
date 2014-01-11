using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oauth.Vk.IApi.PrivateMessaging;
using Oauth.Vk.Infrastructure;
using System.Linq;
using Oauth.Vk.Infrastructure.Enums;

namespace Oauth.Vk.Api.PrivateMessaging
{
    public class VkMessaging : BaseVkApi, IVkMessaging
    {
        public VkMessaging(string accessToken) : base(accessToken)
        {
        }

        public T Messages_Send<T>(string id, string message, VkAttachment[] attachments = null, MessageScopeEnum scope = MessageScopeEnum.User, string title = "")
        {
            var parametrs = new NameValueCollection();

            switch (scope)
            {
                case MessageScopeEnum.User: parametrs.Add("uid", id);
                    break;
                case MessageScopeEnum.Chat: parametrs.Add("chat_id", id);
                    break;
            }

            parametrs.Add("message",message);
            parametrs.Add("title", title);

            if (attachments != null)
            {
                var attachmentsParam = string.Join(",",attachments.Select( e => e.GetValue()));
                parametrs.Add("attachments",attachmentsParam);
            }


            var json = Call("messages.send", parametrs);

            json = JObject.Parse(json).SelectToken("response").ToString();

            return JsonConvert.DeserializeObject<T>(json);  
        }

        public T Messages_Delete<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Messages_DeleteDialog<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Messages_Restore<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Messages_MarkAsNew<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Messages_MarkAsRead<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}
