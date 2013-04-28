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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.UserName = (((UserIdentity) User.Identity).User ?? new User()).FullName;
            base.OnActionExecuting(filterContext);
        }

    }
}