using System;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrainShare.Documents;
using BrainShare.Documents.Data;
using BrainShare.Infostructure;
using BrainShare.Services;
using BrainShare.ViewModels;
using MongoDB.Bson;

namespace BrainShare.Controllers
{
    [RoutePrefix("shell")]
    public class PublicShellController : BaseController
    {
        private readonly ShellUserService _shellUserService;
        private readonly UsersService _usersService;

        public PublicShellController(ShellUserService shellUserService,UsersService usersService)
        {
            _shellUserService = shellUserService;
            _usersService = usersService;
        }

        [GET("create-new")]
        public ActionResult CreateNew()
        {
            var user = _usersService.GetById(UserId);
            var model = new CreateShellViewModel(user.Address.Formatted);
            return View(model);
        }


        [POST("create-new")]
        public ActionResult CreateNew(CreateShellViewModel model, string status)
        {
            if (!ModelState.IsValid)
            {
                model.AddModelStateErrors(ModelState.Keys.SelectMany(key => ModelState[key].Errors), true);
            }
            else
            {
                model.ClearErrors();
                _shellUserService.Insert(new ShellUser
                    {
                        Name = model.Name,
                        ShellAddressData = new ShellAddressData
                            {
                                Country = model.Country,
                                Formatted = model.FormattedAddress,
                                Location = new Location(model.Lat,model.Lng),
                                LocalPath = model.LocalPath,
                                Route = model.Route,
                                StreetNumber = model.StreetNumber
                            },
                        Created = DateTime.UtcNow,
                        Id = ObjectId.GenerateNewId().ToString(),

                    });
            }
            return Json(model);
        }

    }
}
