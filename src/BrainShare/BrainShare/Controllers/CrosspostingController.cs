using System.Linq;
using System.Web.Mvc;
using BrainShare.Domain.Documents;
using BrainShare.Infostructure;
using Brainshare.Infrastructure.Services;
using BrainShare.Services;
using Brainshare.Infrastructure.Settings;
using BrainShare.Utils.Utilities;
using Brainshare.Vk.Api;
using Brainshare.Vk.Helpers;
using MongoDB.Bson;

namespace BrainShare.Controllers
{
    [Authorize]
    public class CrosspostingController : BaseController
    {
        private readonly Settings _settings;
        private readonly LinkedGroupsService _linkedGroups;
        private readonly VkApi _groupApi;
        private readonly BooksService _books;

        private readonly AsyncTaskScheduler _asyncTaskScheduler;

        public CrosspostingController(Settings settings, UsersService usersService, LinkedGroupsService linkedGroups, AsyncTaskScheduler asyncTaskScheduler, BooksService books) : base(usersService)
        {
            _settings = settings;
            _linkedGroups = linkedGroups;
            _asyncTaskScheduler = asyncTaskScheduler;
            _books = books;
            _groupApi = new VkApi(null);
        }

        public ActionResult Index()
        {
            var items = _linkedGroups.GetForUser(UserId);
            var model = items.Select(x => new LinkedGroupItem(x));
            return View(model);
        }

        public ActionResult Authorize(string id)
        {
            var model = new GroupAuthorizeViewModel
            {
                GroupId = id,
                VkBlankUrl = VkAuth.BuildAuthorizeUrlForMobile("4185172")
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Connect(string groupId, string blankPageUrl)
        {
            var token = UrlUtility.ExtractToken(blankPageUrl);
            var linkedGroup = _linkedGroups.GetById(groupId);
            linkedGroup.AccessToken = token;
            linkedGroup.IsActive = true;
            _linkedGroups.Save(linkedGroup);
            return RedirectToAction("Index");
        }

        public ActionResult Add(string url)
        {
            var id = UrlUtility.LastSegment(url).Replace("public", "");
            var group= _groupApi.GetGroupInfo(id);
            var linkedGroup = new LinkedGroup
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = group.Name,
                GroupId = group.Gid,
                OwnerId = UserId
            };
            _linkedGroups.Save(linkedGroup);
            var model = new LinkedGroupPreviewModel
            {
                Id = linkedGroup.Id,
                GroupId = linkedGroup.GroupId,
                Name = linkedGroup.Name,
                Photo = group.Photo
            };
            return View(model);
        }

        public ActionResult Activate(string id)
        {
            var group = _linkedGroups.GetById(id);
            group.IsActive = true;
            _linkedGroups.Save(group);
            return RedirectToAction("Index");
        }

        public ActionResult Disconnect(string id)
        {
            var group = _linkedGroups.GetById(id);
            group.IsActive = false;
            _linkedGroups.Save(group);
            return RedirectToAction("Index");
        }

        public ActionResult PostAll(string id)
        {
            var books = _books.GetAll();
            var group = _linkedGroups.GetById(id);
            foreach (var book in books)
            {
                _asyncTaskScheduler.PostToGroup(group,book);
            }
            group.AllBooksPosted = true;
            _linkedGroups.Save(group);
            return RedirectToAction("Index");
        }
    }
}
