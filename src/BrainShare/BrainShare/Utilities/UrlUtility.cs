using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainShare.Utilities
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

        public static string GetProfileLink(string userId)
        {
            return ApplicationBaseUrl + "/profile/view/" + userId;
        }

        public static string GetBookLink(string bookId)
        {
            return ApplicationBaseUrl + "/books/info/" + bookId;
        }
    }
}