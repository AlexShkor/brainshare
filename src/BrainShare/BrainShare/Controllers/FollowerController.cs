using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Services;
using BrainShare.Utilities;

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

        //shells cant follow 
        [HttpPost]
        public ActionResult Subscribe(string publisherId)
        {
            if (IsShellUser)
            {
                return null;
            }

            var currentUser = _users.GetById(UserId);
            var publisher = _users.GetById(publisherId);

            if (publisher == null)
            {
                var shellPublisher = _shellUserService.GetById(publisherId);
                currentUser.SetPublisher(shellPublisher.MapShellUser());
                shellPublisher.SetFollower(currentUser);
                _shellUserService.Save(shellPublisher);
            }
            else
            {
                 currentUser.SetPublisher(publisher.MapUser());
                publisher.SetFollower(currentUser);
                _users.Save(publisher);
            }
            _users.Save(currentUser);

            return Json("success");
        }

        [HttpPost]
        public ActionResult Unsubscribe(string userId)
        {
            if (IsShellUser)
            {
                return null;
            }

            var currentUser = _users.GetById(UserId);
            currentUser.RemovePublisher(userId);
            var publisher = _users.GetById(userId);
            if (publisher != null)
            {
                publisher.RemoveFollower(currentUser.Id);
                _users.Save(publisher);
            }
            else
            {
                var shellPublisher = _shellUserService.GetById(userId);
                shellPublisher.RemoveFollower(userId);
                _shellUserService.Save(shellPublisher);
            }

            _users.Save(currentUser);

            return Json("success");
        }
    }
}
