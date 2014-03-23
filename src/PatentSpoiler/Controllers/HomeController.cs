using System.Web.Mvc;
using PatentSpoiler.App.Database;

namespace PatentSpoiler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPatentDatabase patentDatabase;

        public HomeController(IPatentDatabase patentDatabase)
        {
            this.patentDatabase = patentDatabase;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchForTerm(string term)
        {
            var results = patentDatabase.NodesForTerm(term);
            ViewBag.results = results;
            return View("Index");
        }

    }
}
