using System.Web.Mvc;

namespace PatentSpoiler.Controllers
{
    public class AdminController : Controller
    {
       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportData()
        {
            
            return Content("Done!");
        }
    }
}