﻿using System.Linq;
using System.Web.Mvc;
using BrainShare.Services;
using BrainShare.ViewModels;

namespace BrainShare.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(UsersService users):base(users)
        {
        }

        public ActionResult Index()
        {
            var model = new UsersFilterModel();

            Title("Пользователи Brainshare");
            return View(model);
        }

        public ActionResult Filter(UsersFilterModel model)
        {
            var filter = model.ToFilter();
            var items = _users.GetByFilter(filter).Select(x => new UserViewModel(x));
            model.UpdatePagingInfo(filter.PagingInfo);
            return Listing(items, model);
        }
    }
}
