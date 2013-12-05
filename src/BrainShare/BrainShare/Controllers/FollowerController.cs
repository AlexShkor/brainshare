using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Services;

namespace BrainShare.Controllers
{
    public class FollowerController : BaseController
    {
        private readonly UsersService _users;
        private readonly ShellUserService _shellUserService;
        private readonly ICommonUserService _commonUserService;

        public FollowerController(UsersService users, ShellUserService shellUserService, ICommonUserService commonUserService)
        {
            _users = users;
            _shellUserService = shellUserService;
            _commonUserService = commonUserService;
        }

        [HttpPost]
        public ActionResult Follow(string userId)
        {
            if (IsShellUser)
            {
                return null;
            }

            var currentUser = _users.GetById(UserId);
            currentUser.SetPublisher(_commonUserService.GetById(userId));
            _users.Save(currentUser);

            return Json("success");
        }

    }
}
