using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Data.Indexes;
using PatentSpoiler.App.Domain;
using Raven.Abstractions.Util;
using Raven.Client;

namespace PatentSpoiler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentStore documentStore;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;

        public HomeController(IPatentStoreHierrachy patentStoreHierrachy, IDocumentStore documentStore)
        {
            this.patentStoreHierrachy = patentStoreHierrachy;
            this.documentStore = documentStore;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchForTerm(string term)
        {
            using (var session = documentStore.OpenSession())
            {
                var query = session.Query<PatentClassification, DocumentsByTitlePartIndex>();

                query = query.Search(x => x.Keywords, RavenQuery.Escape(term, false, true));

                var results = query.Take(10).ToList();

                var viewBagResults = results.Select(x => patentStoreHierrachy.GetDefinitionFor(x.Id));

                ViewBag.results = viewBagResults.ToList();
            }
            
            
            return View("Index");
        }

    }
}
