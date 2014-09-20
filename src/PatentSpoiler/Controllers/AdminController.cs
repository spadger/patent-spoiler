using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.Security;

namespace PatentSpoiler.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPatentDatabaseIndexBuilder patentDatabaseIndexBuilder;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;

        public AdminController(IPatentDatabaseIndexBuilder patentDatabaseIndexBuilder, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.patentDatabaseIndexBuilder = patentDatabaseIndexBuilder;
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

            await patentDatabaseIndexBuilder.IndexCategoriesAsync(root);
            return Content("Done!");
        }
    }
}