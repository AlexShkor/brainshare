using System.Linq;
using System.Web.Mvc;
using BrainShare.Services;
using BrainShare.ViewModels;

namespace BrainShare.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly UsersService _users;

        public UsersController(UsersService users)
        {
            _users = users;
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
