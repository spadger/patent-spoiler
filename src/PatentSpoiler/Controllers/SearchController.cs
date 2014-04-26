using System.Linq;
using System.Web.Mvc;
using PatentSpoiler.App.Data.Queries;
using PatentSpoiler.App.DTOs;

namespace PatentSpoiler.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchForClassificationQuery searchForClassificationQuery;

        public SearchController(ISearchForClassificationQuery searchForClassificationQuery)
        {
            this.searchForClassificationQuery = searchForClassificationQuery;
        }

        public JsonResult Index(string term)
        {
            var result = searchForClassificationQuery.Execute(term, 0, 10);

            var results = result.Select(SearchResult.From).ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}