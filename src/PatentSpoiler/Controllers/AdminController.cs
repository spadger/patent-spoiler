using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Data.ElasticSearch;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.Security;

namespace PatentSpoiler.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPatentSchemeSearchIndexBuilder patentSchemeSearchIndexBuilder;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;

        public AdminController(IPatentSchemeSearchIndexBuilder patentSchemeSearchIndexBuilder, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.patentSchemeSearchIndexBuilder = patentSchemeSearchIndexBuilder;
            this.patentStoreHierrachy = patentStoreHierrachy;
        }

        [AuthoriseRoles(UserRole.Admin)]
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ImportData()
        {
            var root = patentStoreHierrachy.Root;

            await patentSchemeSearchIndexBuilder.IndexCategoriesAsync(root);
            return Content("Done!");
        }
    }
}