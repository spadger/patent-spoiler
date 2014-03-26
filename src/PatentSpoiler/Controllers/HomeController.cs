using System.Web.Mvc;
using PatentSpoiler.App.Database;
using Raven.Client;

namespace PatentSpoiler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPatentDatabase patentDatabase;
        private readonly IDocumentStore documentStore;

        public HomeController(IPatentDatabase patentDatabase, IDocumentStore documentStore)
        {
            this.patentDatabase = patentDatabase;
            this.documentStore = documentStore;
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
