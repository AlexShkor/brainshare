using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using BrainShare.Admin.Models;

namespace BrainShare.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> UserManager;

        public List<ApplicationUser> Admins = new List<ApplicationUser>()
        {
            new ApplicationUser()
            {
                Id = "dhfiegrshjrgert",
                Password = "itjh1990",
                UserName = "admin",
                PasswordHash = new PasswordHasher().HashPassword("irjh1990")
            }
        };

        public AccountController()
        {
            UserManager = new UserManager<ApplicationUser>(new MyUserStore(Admins));
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }


        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }

    public class MyUserStore : IUserPasswordStore<ApplicationUser>
    {
        private readonly List<ApplicationUser> _admins;

        public MyUserStore(List<ApplicationUser> admins)
        {
            _admins = admins;
        }

        public void Dispose()
        {
        }

        public async Task CreateAsync(ApplicationUser user)
        {
            await Task.Factory.StartNew(() => _admins.Add(user));
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            await DeleteAsync(user);
            await CreateAsync(user);
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            await Task.Factory.StartNew(() => _admins.RemoveAll(x => x.Id == user.Id));
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var result = await Task<ApplicationUser>.Factory.StartNew(() => _admins.FirstOrDefault(x => userId == x.Id));
            return result;
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var result =  await Task<ApplicationUser>.Factory.StartNew(() => _admins.FirstOrDefault(x => userName == x.UserName));
            return result;
        }

        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            var current = await FindByIdAsync(user.Id);
            current.PasswordHash = passwordHash;
        }

        public async Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            var current = await FindByIdAsync(user.Id);
            return current.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            var current = await FindByIdAsync(user.Id);
            return current.Password != null;
        }
    }
}