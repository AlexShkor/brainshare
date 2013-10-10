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
                var request = HttpContext.Current.Request;
                var appUrl = HttpRuntime.AppDomainAppVirtualPath;
                var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
                return baseUrl;
            }
        }
    }
}