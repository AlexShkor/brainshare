using Facebook;

namespace Brainshare.Infrastructure.Facebook
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