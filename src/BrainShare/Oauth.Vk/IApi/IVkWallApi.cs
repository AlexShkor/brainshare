using Oauth.Vk.Infrastructure;
using Oauth.Vk.Infrastructure.Attachments;
using Oauth.Vk.Infrastructure.Enums;

namespace Oauth.Vk.IApi
{
    public interface IVkWallApi
    {
        T Wall_Get<T>();
        T Wall_GetComments<T>();
        T Wall_GetById<T>();
        T Wall_Post<T>(string ownerId, string message, string url, StatusName fromGroup, GroupPostSign signed);
        T Wall_GetLikes<T>();
    }
}
