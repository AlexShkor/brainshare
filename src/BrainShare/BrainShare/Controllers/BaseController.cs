using System;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class BaseController : Controller
    {
        public string UserId
        {
            get
            {
                return  (((UserIdentity) User.Identity).User ?? new User()).Id;
            }
        }

        protected void Title(string title)
        {
            ViewBag.Title = title;
        }

        protected void Subtitle(string subtitle)
        {
            ViewBag.Subtitle = subtitle;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var action = (string)RouteData.Values["action"];
            var controller = (string)RouteData.Values["controller"];
            Title(action.Equals("index",StringComparison.InvariantCultureIgnoreCase) ? controller : action);
            ViewBag.UserName = (((UserIdentity) User.Identity).User ?? new User()).FullName;
            base.OnActionExecuting(filterContext);
        }

    }
}