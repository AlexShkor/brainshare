using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Documents;
using BrainShare.Services;

namespace BrainShare.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UsersService _users;

        public UsersController(UsersService users)
        {
            _users = users;
        }

        public ActionResult Index()
        {
            var model = _users.GetAll().Select(x => new UserViewModel(x)).OrderByDescending(x => x.Rating).ToList();
            return View(model);
        }
    }

    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Registered { get; set; }

        public int Rating { get; set; }
        public UserViewModel()
        {

        }

        public UserViewModel(User user)
        {
            UserId = user.Id;
            Username = user.FullName;
            Address = user.Address.Original;
            Avatar = user.AvatarUrl ?? Constants.DefaultAvatarUrl;
            Registered = user.Registered.ToShortDateString();
            Rating = user.GetSummaryVotes();
        }
    }
}
