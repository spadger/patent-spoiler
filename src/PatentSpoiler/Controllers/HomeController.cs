using System.Web.Mvc;
using PatentSpoiler.App.Database;
using Raven.Client;

namespace PatentSpoiler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPatentDatabaseLoader patentDatabaseLoader;
        private readonly IDocumentStore documentStore;

        public HomeController(IPatentDatabaseLoader patentDatabaseLoader, IDocumentStore documentStore)
        {
            this.patentDatabaseLoader = patentDatabaseLoader;
            this.documentStore = documentStore;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchForTerm(string term)
        {
            //var results = patentDatabase.(term);
            //ViewBag.results = results;
            return View("Index");
        }

    }
}
