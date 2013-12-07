using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Services;
using BrainShare.ViewModels;

namespace BrainShare.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UsersService _users;

        public UsersController(UsersService users)
        {
            _users = users;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new UsersFilterModel
                {
                    Users = _users.GetAll().Select(x => new UserViewModel(x)).OrderByDescending(x => x.Rating).ToList()
                };

            return View(model);
        }


        [AllowAnonymous]
        public ActionResult Filter(UsersFilterModel model)
        {
            var filter = model.ToFilter();
            var items = _users.GetByFilter(filter).Select(x => new UserViewModel(x));
            model.UpdatePagingInfo(filter.PagingInfo);
            return Listing(items, model);
        }
    }
}
