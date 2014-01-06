namespace Oauth.Vk.IApi
{
    public interface IVkGeolocation
    {
        T Places_Add<T>();
        T Places_GetById<T>();
        T Places_Search<T>();
        T Places_Checkin<T>();
        T Places_GetCheckins<T>();
        T Places_GetTypes<T>();
        T Places_GetCountries<T>();
        T Places_GetCities<T>();
        T Places_GetRegions<T>();
        T Places_GetCityById<T>(params string[] cids);
        T Places_GetCountryById<T>(params string[] cids);
        T Places_GetStreetById<T>();
    }
}
