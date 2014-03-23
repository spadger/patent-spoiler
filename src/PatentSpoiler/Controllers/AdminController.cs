using System.Web.Mvc;
using PatentSpoiler.App.Import;
using PatentSpoiler.App.Import.Config;
using PatentSpoiler.Models;
using System.Linq;
namespace PatentSpoiler.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDefinitionImporter importer;
        private readonly ImporterSettings importerSettings;

        public AdminController(IDefinitionImporter importer, ImporterSettings importerSettings)
        {
            this.importer = importer;
            this.importerSettings = importerSettings;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportData()
        {
            var root = importer.Import(importerSettings.DocumentsPath, importerSettings.RootDocumentFileName);
            return Content("Done!");
        }
    }
}