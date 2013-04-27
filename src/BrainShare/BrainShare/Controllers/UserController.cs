using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.ViewModels;

namespace BrainShare.Controllers
{
    public class UserController : Controller
    {
        public IAuthentication Auth { get; set; }

        public UserController(IAuthentication auth)
        {
            Auth = auth;
        }

        private User CurrentUser
        {
            get { return ((IUserProvider)Auth.CurrentUser.Identity).User; }
        }

        public ActionResult UserLogin()
        {
            return View(CurrentUser);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginView());
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Email, loginView.Password, loginView.IsPersistent);
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState["Password"].Errors.Add("Such e-mail or password are not registered");
            }
            return View(loginView);
        }

        public ActionResult Logout()
        {
            Auth.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterView newUser)
        {
            return View();
        }
    }
}

