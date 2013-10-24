using System.Web.Mvc;

namespace BrainShare.Facebook
{
    public class FacebookAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var accessToken = httpContext.Session[SessionKeys.FbAccessToken] as string;
            if (string.IsNullOrWhiteSpace(accessToken))
                return false;
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //var mode = int.Parse(filterContext.Controller.ValueProvider.GetValue("mode").AttemptedValue);
            //var returnUrl = filterContext.Controller.ValueProvider.GetValue("returnUrl").AttemptedValue;
            //filterContext.Result = new RedirectResult("/user/GetFacebookToken?returnUrl=" + returnUrl + "?mode=" + mode);
            filterContext.Result = new RedirectResult("/user/GetFacebookToken?mode=" + FacebookCallbackMode.AuthorizeWithFacebook);
        }
    }
}