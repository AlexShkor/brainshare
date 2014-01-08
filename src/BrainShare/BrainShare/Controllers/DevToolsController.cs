﻿using System.Linq;
using System.Web.Mvc;
using BrainShare.Domain.Documents.Data;
using BrainShare.Infostructure;
using BrainShare.Services;

namespace BrainShare.Controllers
{
    [Authorize]
    public class DevToolsController : BaseController
    {
        private readonly CryptographicHelper _cryptographicHelper;

        public DevToolsController(UsersService usersService, CryptographicHelper cryptographicHelper) : base(usersService)
        {
            _cryptographicHelper = cryptographicHelper;
        }
   

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MigratePasswords()
        {
            var allUsers = _users.GetAll().Where(e => e.Salt == null).ToList();

            var total = 0;
            foreach (var user in allUsers)
            {
                var salt = _cryptographicHelper.GenerateSalt();

                user.Password = _cryptographicHelper.GetPasswordHash(user.Password,salt);
                user.Salt = salt;

                _users.Save(user);

                total++;
            }

            return Json(string.Format("success: total changes {0}", total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MigrateAccounts()
        {
            var allUsers = _users.GetAll().ToList();

            foreach (var user in allUsers)
            {
                if (user.FacebookId != null && user.LoginServices.All(e => e.LoginType != LoginServiceTypeEnum.Facebook))
                {
                    var loginEntry = new LoginService
                        {
                            AccessToken = user.FacebookAccessToken,
                            LoginType = LoginServiceTypeEnum.Facebook,
                            ServiceUserId = user.FacebookId
                        };

                    user.LoginServices.Add(loginEntry);
                    _users.Save(user);
                }

                if (user.Email != null && user.LoginServices.All(e => e.LoginType != LoginServiceTypeEnum.Email))
                {
                    var loginEntry = new LoginService
                    {
                        LoginType = LoginServiceTypeEnum.Email,
                        ServiceUserId = user.Email
                    };

                    user.LoginServices.Add(loginEntry);
                    _users.Save(user);
                }
            }

           return RedirectToAction("Index");
        }
    }
}
