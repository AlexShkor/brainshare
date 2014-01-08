using System;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using BrainShare.Infostructure;
using BrainShare.Services;
using BrainShare.Utils.Utilities;
using BrainShare.ViewModels;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Services;
using Brainshare.Infrastructure.Settings;
using MongoDB.Bson;
using System.Linq;

namespace BrainShare.Controllers
{
    [AttributeRouting.RoutePrefix("user")]
    public class UserController : BaseController
    {
        private readonly Settings _settings;
        public IAuthentication Auth { get; set; }
        private readonly ShellUserService _shellUserService;
        private readonly NewsService _news;
        private readonly MailService _mailService;
        private readonly CryptographicHelper _cryptoHelper;

        public string VkCallbackUri
        {
            get
            {
                return UrlUtility.ApplicationBaseUrl + Url.Action("VkCallback");
            }
        }

        public UserController(IAuthentication auth, UsersService users, ShellUserService shellUserService, CryptographicHelper cryptoHelper, Settings settings, NewsService news, MailService mailService):base(users)
        {
            _settings = settings;
            _shellUserService = shellUserService;
            _cryptoHelper = cryptoHelper;
            _news = news;
            _mailService = mailService;
            Auth = auth;
        }

        private CommonUser CurrentUser
        {
            get { return ((IUserProvider)Auth.CurrentUser.Identity).User; }
        }

        public ActionResult UserLogin()
        {
            return View(_users.GetById(CurrentUser.Id));
        }

        [HttpGet]
        public ActionResult Login()
        {
            Title("Вход в систему");
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

                ModelState["Password"].Errors.Add("Такой e-mail или пароль не зарегистрирован");
            }
            return View(loginView);
        }

        public ActionResult Logout()
        {
            // does we need?
            Auth.Logout();
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            Title("Зарегистрироваться");
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var anyUser = _users.GetUserByLoginServiceInfo(LoginServiceTypeEnum.Email, model.Email);
                if (anyUser == null)
                {
                    var salt = _cryptoHelper.GenerateSalt();
                    var hashedPassword = _cryptoHelper.GetPasswordHash(model.Password,salt);

                    var newUser = new User()
                                      {
                                          Id = GetIdForUser(),
                                          FirstName = model.FirstName,
                                          Address = new AddressData(model.original_address,model.formatted_address,model.country,model.locality),
                                          LastName = model.LastName,
                                          Email = model.Email.ToLower(),
                                          Password = hashedPassword,
                                          Registered = DateTime.Now,
                                          Salt = salt
                                      };

                    _users.AddUser(newUser);

                    _mailService.SendWelcomeMessage(newUser);
                }
                else
                {
                    ModelState.AddModelError("Email", "Пользователь с таким e-mail уже существует");
                }
            }
            return JsonModel(model);
        }

        [HttpPost]
        public ActionResult RegisterAsBookShell(CreateShellViewModel model)
        {
            if (ModelState.IsValid)
            {
                var anyUser = _users.GetUserByLoginServiceInfo(LoginServiceTypeEnum.Email, model.Email);
                if (anyUser == null)
                {
                    var salt = _cryptoHelper.GenerateSalt();
                    var hashedPassword = _cryptoHelper.GetPasswordHash(model.Password, salt);

                    var user = new ShellUser
                        {
                            Name = model.Name,
                            ShellAddressData = new ShellAddressData
                                {
                                    Country = model.Country,
                                    Formatted = model.FormattedAddress,
                                    Location = new Location(model.Lat, model.Lng),
                                    LocalPath = model.LocalPath,
                                    Route = model.Route,
                                    StreetNumber = model.StreetNumber
                                },
                            Created = DateTime.UtcNow,
                            Id = ObjectId.GenerateNewId().ToString(),
                            Password = hashedPassword,
                            Email = model.Email.ToLower(),
                            Salt = salt
                        };

                    _shellUserService.Save(user);

                    return Json(model);
                }

                ModelState.AddModelError("Email", "Пользователь с таким e-mail уже существует");
            }
      
            return JsonModel(model);
        }

        [HttpGet]
        public ActionResult RegisterAsBookshell()
        {
            Title("Зарегистрироваться в качестве 'полки'");
            return View(new CreateShellViewModel());
        }
        


        public static string GetIdForUser()
        {
            return ObjectId.GenerateNewId().ToString();
        }


      


        [GET("News")]
        public ActionResult News()
        {
            if (!IsShellUser)
            {
               var user = _users.GetById(UserId);
               var news = _news.GetByIds(user.News.Select(n => n.Id));

               var model = new NewsViewModel(user.News, news);
               return View(model);
            }
          
        
            return View();
        }

        [GET("Friends/{userId}")]
        public ActionResult Friends(string userId)
        {
            if (IsShellUser)
            {
                //Todo: implementation will be based on future needs
                return View("ShellFriends");
            }
            //Todo: future implementation will be based on user settings
            userId = userId ?? UserId;
            var user = _users.GetById(userId);
            var publishers = _users.GetByIds(user.Publishers);

            Title("На кого я подписан");
            return View(new PublishersViewModel(publishers,user.FullName, userId == UserId,_settings.ActivityTimeoutInMinutes));
        }
    }
}

