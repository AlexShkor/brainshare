using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.ViewModels.Base;

namespace BrainShare.Controllers
{
    public class BaseController : Controller
    {
        public string UserId
        {
            get
            {
                return (((UserIdentity)User.Identity).User ?? new User()).Id;
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
            Title(action.Equals("index", StringComparison.InvariantCultureIgnoreCase) ? controller : action);
            ViewBag.UserId = UserId;
            ViewBag.UserName = (((UserIdentity)User.Identity).User ?? new User()).FullName;
            ViewBag.IsFacebookAccount = (((UserIdentity)User.Identity).User ?? new User()).IsFacebookAccount;
            base.OnActionExecuting(filterContext);
        }

        protected JsonResult JsonErrors()
        {
            return Json(new
            {
                Errors = GetErorsModel()
            });
        }


        protected JsonResult JsonModel<T>(T model) where T : BaseViewModel
        {
            model.Errors = GetErorsModel().ToList();
            return Json(model);
        }

        protected IEnumerable<ValidationError> GetErorsModel()
        {
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    yield return new ValidationError(error.ErrorMessage, state.Key);
                }
            }
        }
    }
}