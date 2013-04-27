using System.Web;
using Facebook;

namespace BrainShare
{
    public class FacebookClientFactory
    {
        public FacebookClient GetClient()
        {
            var facebookClient = new FacebookClient();
            try
            {
                var token = HttpContext.Current.Session[SessionKeys.FbAccessToken] as string;
                facebookClient.AccessToken = token;
            }
            catch
            {

            }
            return facebookClient;
        }
    }
}