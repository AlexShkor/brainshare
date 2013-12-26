using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BrainShare.Controllers
{
    [AttributeRouting.RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        [GET("500")]
        public ActionResult Error500()
        {
            return View("ErrorPages/500");
        }

    }
}
