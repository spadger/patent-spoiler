using System;

using System.Web;
using System.Web.Mvc;

namespace PatentSpoiler.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return Redirect("/admin/importdata");
        }

    }
}
