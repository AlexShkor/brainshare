﻿namespace Oauth.Vk.IApi
{
    public interface IVkUserApi
    {
        T IsAppUser<T>();
        T Users_Get<T>(string[] fields, string[]uids);
        T Users_Search<T>();
        T GetUserSettings<T>();
        T LikesGetList<T>();
    }
}