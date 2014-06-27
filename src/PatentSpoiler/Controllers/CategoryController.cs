using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App.Data.Indexes;
using PatentSpoiler.App.Data.Queries;
using PatentSpoiler.App.Domain.Patents;
using Raven.Client;

namespace PatentSpoiler.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRetrivePatentsForClassificationQuery patentsForClassificationQuery;
        private readonly IDocumentStore documentStore;

        public CategoryController(IRetrivePatentsForClassificationQuery patentsForClassificationQuery, IDocumentStore documentStore)
        {
            this.patentsForClassificationQuery = patentsForClassificationQuery;
            this.documentStore = documentStore;
        }

        [Route("category/{*category}")]
        public async Task<ActionResult> Index(string category, int page = 1, int pageSize = 10)
        {
            var results = await patentsForClassificationQuery.ExecuteAsync(category, page, pageSize);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [Route("category/poo/{*category}")]
        public async Task<ActionResult> Index(string category)
        {
            using (var session = documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(new PatentableEntity {Category = category, Title = Guid.NewGuid().ToString()});
                await session.SaveChangesAsync();
            }

            return Content("Stored: " + category);
        }
    }
}