using System;
using System.Collections.Concurrent;
using System.Linq;
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

        public static string ResizeAvatar(string originalUrl, int size)
        {
            if (originalUrl == null)
            {
                return originalUrl;
            }
            if (originalUrl.Contains(".vk."))
            {
                //don't have API for doing this dynamicly
                return originalUrl;
            }
            if (originalUrl.Contains("facebook"))
            {
                return originalUrl.Substring(0, originalUrl.IndexOf("?")) + string.Format("?width={0}&height={0}", size);
            }
            return originalUrl.Insert(originalUrl.LastIndexOf("/"), "/w_" + size/200f);
        }

        public static string LastSegment(string url)
        {
            return (new Uri(url).Segments).Last();
        }

        public static string ExtractVkGroupId(string url)
        {
            for (int i = url.Length - 1; i > 0; i--)
            {
                if (!char.IsDigit(url,i))
                {
                    return url.Substring(i + 1);
                }
            }

            throw new Exception("wrong group url format");
        }


        public static string ExtractToken(string url)
        {
            var uri = new Uri(url.Replace("#", "?"));
            return HttpUtility.ParseQueryString(uri.Query).Get("access_token");
        }

        public static DateTime ExtractExpirationDate(string url)
        {
            var uri = new Uri(url.Replace("#", "?"));
            var value = HttpUtility.ParseQueryString(uri.Query).Get("expires_in");
            return DateTime.UtcNow.AddSeconds(int.Parse(value));
        }

        public static string ParseVkGroupId(string url)
        {
            var id = LastSegment(url);
            return id;
        }
    }
}