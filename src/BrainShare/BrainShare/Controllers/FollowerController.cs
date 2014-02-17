﻿using System.Web.Mvc;
using BrainShare.Infrastructure.Utilities;
using BrainShare.Services;
using Brainshare.Infrastructure.Services;

namespace BrainShare.Controllers
{
    public class FollowerController : BaseController
    {
        private readonly ShellUserService _shellUserService;

        public FollowerController(UsersService users, ShellUserService shellUserService):base(users)
        {
            _shellUserService = shellUserService;
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
                shellPublisher.SetFollower(currentUser.Id);
                _shellUserService.Save(shellPublisher);
            }
            else
            {
                 currentUser.SetPublisher(publisher.MapUser());
                 publisher.SetFollower(currentUser.Id);
                _users.Save(publisher);
            }
            _users.Save(currentUser);

            return Json("success");
        }

        [HttpPost]
        public ActionResult Unsubscribe(string userId)
        {
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
