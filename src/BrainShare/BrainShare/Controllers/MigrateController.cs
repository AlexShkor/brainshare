using System.Web.Mvc;
using BrainShare.Infostructure;
using BrainShare.Services;

namespace BrainShare.Controllers
{
    public class MigrateController : Controller
    {
        private UsersService _users;
        private CryptographicHelper _cryptographicHelper;

        public MigrateController(UsersService usersService, CryptographicHelper cryptographicHelper)
        {
            _users = usersService;
            _cryptographicHelper = cryptographicHelper;
        }

        public ActionResult Index(string password)
        {
            if (!"stringpassword".Equals(password))
            {
                return Json("wrong credentials",JsonRequestBehavior.AllowGet);
            }
            var total = 0;
            foreach (var user in _users.GetAll())
            {
                user.Password = _cryptographicHelper.GetPasswordHash(user.Password);
                user.Email = user.Email.ToLower();
                total++;
            }

            return Json(string.Format("success: total changes {0}",total),JsonRequestBehavior.AllowGet);
        }

    }
}
