namespace Brainshare.Infrastructure.VK
{
    public interface IVkApi:IVkUserApi
    {
    }

    public interface IVkUserApi
    {
        T IsAppUser<T>();
        T Users_Get<T>();
        T Users_Search<T>();
        T GetUserSettings<T>();
        T LikesGetList<T>();
    }
}
