using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oauth.Vk.IApi;
using Oauth.Vk.Infrastructure;

namespace Oauth.Vk.Api
{
    public class VkGeolocationApi : BaseVkApi, IVkGeolocation
    {
        public VkGeolocationApi(string accessToken) : base(accessToken)
        {
        }

        public T Places_Add<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Places_GetById<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Places_Search<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Places_Checkin<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Places_GetCheckins<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Places_GetTypes<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Places_GetCountries<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Places_GetCities<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Places_GetRegions<T>()
        {
            throw new System.NotImplementedException();
        }

        public T Places_GetCityById<T>(params string[] cids)
        {
            var parametrs = new NameValueCollection { { "cids", string.Join(",", cids) } };
            var json = Call("places.getCityById", parametrs, AccessToken);

            json = JObject.Parse(json).SelectToken("response").ToString();

            return JsonConvert.DeserializeObject<T>(json);  
        }

        public T Places_GetCountryById<T>(params string[] cids)
        {
            var parametrs = new NameValueCollection { { "cids", string.Join(",", cids) } };
            var json = Call("places.getCountryById", parametrs, AccessToken);

            json = JObject.Parse(json).SelectToken("response").ToString();

            return JsonConvert.DeserializeObject<T>(json);  
        }

        public T Places_GetStreetById<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}
