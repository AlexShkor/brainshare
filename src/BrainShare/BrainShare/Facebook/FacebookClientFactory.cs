using Facebook;

namespace BrainShare.Facebook
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