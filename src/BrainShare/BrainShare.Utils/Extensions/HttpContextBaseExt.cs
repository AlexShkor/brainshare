using System.Web;

namespace BrainShare.Utils.Extensions
{
    public static class HttpContextBaseExt
    {
        /// <summary>
        /// Returns the base url for the website
        /// </summary>
        /// <param name="httpContext">HttpContextBase for the website</param>
        /// <returns>Base URL (with trailing forward-slash)</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="httpContext"/> is null</exception>
        public static string BaseUrl(this HttpContextBase httpContext)
        {
            Ensure.Argument.NotNull(httpContext, "httpContext");
            var request = httpContext.Request;
            return (request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + '/').Trim();
        }
    }
}
