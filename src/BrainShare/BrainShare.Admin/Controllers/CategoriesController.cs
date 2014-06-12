using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainShare.Domain.Documents;
using Brainshare.Infrastructure.Services;
using MongoDB.Bson;

namespace BrainShare.Admin.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly CategoriesService _categories;

        public CategoriesController(CategoriesService categories)
        {
            _categories = categories;
        }

        public ActionResult Index()
        {
            return View(_categories.GetAll());
        }

        public ActionResult Add(string name)
        {
            var category  = _categories.Find(name);
            if (category == null)
            {
                _categories.Insert(new Category
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = name
                });
            }
            return RedirectToAction("Index");
        }
    }
}