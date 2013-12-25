using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Services;
using BrainShare.ViewModels.Base;
using Brainshare.Infrastructure.Authentication;

namespace BrainShare.Controllers
{
    public class BaseController : Controller
    {
        public readonly UsersService _users;

        public BaseController(UsersService usersService)
        {
            _users = usersService;
        }

        public string UserId
        {
            get
            {
                return (((UserIdentity)User.Identity).User ?? new CommonUser()).Id;
            }
        }

        public string UserName
        {
            get
            {
                return (((UserIdentity)User.Identity).User ?? new CommonUser()).FullName;
            }
        }

        public bool IsShellUser
        {
            get
            {
                return (((UserIdentity)User.Identity).User ?? new CommonUser()).OriginalType == typeof(ShellUser);
            }
        }

        public bool IsFacebookAccountWithoutPassword
        {
            get
            {
                var user = ((UserIdentity) User.Identity).User ?? new CommonUser();
                return user.IsFacebookAccount && user.Password == null;
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
            ViewBag.UserName = (((UserIdentity)User.Identity).User ?? new CommonUser()).FullName;
            ViewBag.IsFacebookAccount = (((UserIdentity)User.Identity).User ?? new CommonUser()).IsFacebookAccount;

            _users.SetLastVisitedDate(DateTime.UtcNow, UserId);

            base.OnActionExecuting(filterContext);
        }

        protected JsonResult JsonErrors()
        {
            return Json(new
            {
                Errors = GetErorsModel()
            });
        }

        protected JsonResult JsonSuccess()
        {
            return Json(new
            {
                Success = true
            });
        }

        protected JsonResult JsonError(string message)
        {
            return Json(new
            {
                Error = message
            });
        }

        public JsonResult Listing(object items, object filter)
        {
            return Json(new {
                                Items = items,
                                Filter = filter
                            }, JsonRequestBehavior.AllowGet);
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