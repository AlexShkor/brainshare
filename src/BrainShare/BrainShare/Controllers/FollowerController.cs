using System.Web.Mvc;
using BrainShare.Infrastructure.Utilities;
using BrainShare.Services;
using Brainshare.Infrastructure.Services;

namespace BrainShare.Controllers
{
    public class FollowerController : BaseController
    {

        public FollowerController(UsersService users)
            : base(users)
        {
        }

        [HttpPost]
        public ActionResult Subscribe(string publisherId)
        {
            var currentUser = _users.GetById(UserId);
            var publisher = _users.GetById(publisherId);
            currentUser.SetPublisher(publisher.MapUser());
            publisher.SetFollower(currentUser.Id);
            _users.Save(publisher);
            _users.Save(currentUser);

            return Json("success");
        }

        [HttpPost]
        public ActionResult Unsubscribe(string userId)
        {
            var currentUser = _users.GetById(UserId);
            currentUser.RemovePublisher(userId);
            var publisher = _users.GetById(userId);
            publisher.RemoveFollower(currentUser.Id);
            _users.Save(publisher);
            _users.Save(currentUser);
            return Json("success");
        }
    }
}
