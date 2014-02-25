using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatentSpoiler.App.Import;

namespace PatentSpoiler.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDefinitionImporter importer;

        public AdminController(IDefinitionImporter importer)
        {
            this.importer = importer;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportData()
        {
            importer.Import();
            return Content("Done!");
        }

    }
}
