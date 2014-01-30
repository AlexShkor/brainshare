﻿using System.Text.RegularExpressions;
using System.Web;
using BrainShare.Utils.Extensions;

namespace BrainShare.Utils.Utilities
{
    public static class UrlUtility
    {
        public static readonly Regex UrlRegex = new Regex(@"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)", RegexOptions.Compiled);

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

        public static bool IsUrl(this string @string)
        {
            return @string.HasValue() && UrlRegex.IsMatch(@string);
        }

    }
}