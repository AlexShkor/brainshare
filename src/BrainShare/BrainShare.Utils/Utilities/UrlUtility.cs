using System.Security.Policy;
using System.Web;

namespace BrainShare.Utils.Utilities
{
    public static class UrlUtility
    {       
        public static string ApplicationBaseUrl
        {
            get
            {
                var httpContext = HttpContext.Current;
                var url = httpContext.Request.Url.AbsoluteUri.Replace(httpContext.Request.Url.PathAndQuery, "");
                if (httpContext.Request.Url.Host != "localhost")
                {
                    url = url.Replace(":" + httpContext.Request.Url.Port, "");
                }

                return url;
            }
        }

        public static string CurrentUrl
        {
            get { return ApplicationBaseUrl + HttpContext.Current.Request.Url.PathAndQuery; }
        }

        public static string GetProfileLink(string userId, string  applicationBaseUrl)
        {
            return applicationBaseUrl + "/profile/view/" + userId;
        }

        public static string GetBookLink(string bookId, string applicationBaseUrl)
        {
            return applicationBaseUrl + "/books/info/" + bookId;
        }

        public static string EmailApproveLink(string hashedUserId, string email, string applicationBaseUrl)
        {
            return applicationBaseUrl + string.Format("/user/confirmemail?userId={0}&&email={1}", HttpUtility.UrlEncode(hashedUserId), HttpUtility.UrlEncode(email));
        }

        public static string GetExchangeArrowUrl(int direction, int color)
        {
            string arrow_name = "";

            switch (direction)
            {
                case 0:
                    arrow_name = "right";
                    break;
                case 1:
                    arrow_name = "left";
                    break;  
                
                default : arrow_name = "left";
                    break;
            }

            switch (color)
            {
                case 0:
                    arrow_name += "_green.png";
                    break;
                case 1:
                    arrow_name += "_blue.png";
                    break;

                default: arrow_name += "_blue.png";
                    break;
            }
            

            return "/Images/" + arrow_name;
        }
   
    }
}