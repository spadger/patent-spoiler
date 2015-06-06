using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App.Data.ElasticSearch.Queries;
using PatentSpoiler.App.Data.Queries;
using PatentSpoiler.App.DTOs;

namespace PatentSpoiler.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchByClassificationQuery searchByClassificationQuery;
        private readonly ISearchByEntityContentQuery searchByEntityContentQuery;

        public SearchController(ISearchByClassificationQuery searchByClassificationQuery, ISearchByEntityContentQuery searchByEntityContentQuery)
        {
            this.searchByClassificationQuery = searchByClassificationQuery;
            this.searchByEntityContentQuery = searchByEntityContentQuery;
        }

        public ActionResult Index(string term)
        {
            var vm = new SearchViewModel {Term = term};
            return View(vm);
        }

        public async Task<JsonResult> ByClassification(string term, int skip = 0)
        {
            var result = await searchByClassificationQuery.ExecuteAsync(term, skip, 10);

            var results = Page.Of(result.Items.Select(SearchResult.From), result.Count);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ByEntityContent(string term, int skip = 0)
        {
            var results = await searchByEntityContentQuery.ExecuteAsync(term, skip, 10);

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}