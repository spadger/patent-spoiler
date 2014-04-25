using System.Linq;
using System.Web.Mvc;
using PatentSpoiler.App.Data.Queries;
using PatentSpoiler.App.DTOs;

namespace PatentSpoiler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchForClassificationQuery searchForClassificationQuery;

       
        public HomeController(ISearchForClassificationQuery searchForClassificationQuery)
        {
            this.searchForClassificationQuery = searchForClassificationQuery;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SearchForTerm(string term)
        {
            var result = searchForClassificationQuery.Execute(term, 0, 10);

            return Json(result.Select(SearchResultTree.From), JsonRequestBehavior.AllowGet);
        }
    }
}
