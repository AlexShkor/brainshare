using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Models;
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

        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
            }
            return View(model);
        }
    }



    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

