using System.Web.Mvc;
using BrainShare.Documents;
using Newtonsoft.Json;

namespace BrainShare.Controllers
{
    public class BooksController : Controller
    {
        //
        // GET: /Books/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Give(string book)
        {
            var doc = JsonConvert.DeserializeObject<Book>(book);

            return Json(new { doc.Id });
        }
    }
}
