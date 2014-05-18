using System.Web.Mvc;

namespace PatentSpoiler.Controllers
{
    public class CategoryController : Controller
    {
        [Route("category/{id}")]
        public ActionResult Index(string id)
        {
            return Content(id);
        }
    }
}