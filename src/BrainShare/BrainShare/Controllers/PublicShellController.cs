using System;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrainShare.Documents;
using BrainShare.Services;
using BrainShare.ViewModels;
using MongoDB.Bson;

namespace BrainShare.Controllers
{
    [RoutePrefix("shell")]
    public class PublicShellController : BaseController
    {
        private readonly PublicShellService _shellService;
        private readonly UsersService _usersService;

        public PublicShellController(PublicShellService shellService,UsersService usersService)
        {
            _shellService = shellService;
            _usersService = usersService;
        }

        [GET("Index")]
        public ActionResult Index(string userId)
        {
            var id = userId ?? UserId;
            var shells = _shellService.GetUserShells(id);
            var shellsViewModel = new ShellsViewModel(shells);

            return View(shellsViewModel);
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
                //TODO: Леша, что за бред ты тут написал? (с) Егор
                _shellService.Insert(new PublicShell
                    {
                        Name = model.Name,
                        LocalPath = model.LocalPath,
                        Created = DateTime.UtcNow,
                        Id = ObjectId.GenerateNewId().ToString(),
                        CreatorId = UserId,
                        Lat = model.Lat,
                        Lng = model.Lng,
                        FormattedAddress = model.FormattedAddress,
                        Country = model.Country,
                        Route = model.Route,
                        StreetNumber = model.StreetNumber
                    });
            }
            return Json(model);
        }

    }
}
