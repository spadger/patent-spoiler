using System.Web.Mvc;

namespace PatentSpoiler.Controllers
{
    public class TestController : Controller
    {
        public ActionResult A()
        {
            return Content("a");
        }

        [Authorize]
        public ActionResult B()
        {
            return Content("b");
        }
    }
}