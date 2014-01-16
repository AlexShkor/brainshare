using Oauth.Vk.Infrastructure;
using Oauth.Vk.Infrastructure.Enums;

namespace Oauth.Vk.IApi.PrivateMessaging
{
    public interface IVkMessaging
    {
        T Messages_Send<T>(string id, string message, VkAttachment[] attachments = null, MessageScopeEnum scope = MessageScopeEnum.User, string title = "");
        T Messages_Delete<T>();
        T Messages_DeleteDialog<T>();
        T Messages_Restore<T>();
        T Messages_MarkAsNew<T>();
        T Messages_MarkAsRead<T>();
    }
}
