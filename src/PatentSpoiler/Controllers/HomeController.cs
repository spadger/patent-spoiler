using System.Web.Mvc;
using PatentSpoiler.App.Security;

namespace PatentSpoiler.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
