using System.Web;
using Facebook;

namespace BrainShare
{
    public class FacebookClientFactory
    {
        public FacebookClient GetClient()
        {
            var facebookClient = new FacebookClient();
            return facebookClient;
        }
    }
}