using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App;
using PatentSpoiler.App.Data.Queries;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs;
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
        public async Task<ActionResult> Index(string category)
        {
            return View(new CategoryListDisplayViewModel{Category = category});
        }

        [Route("category/list/{*category}")]
        public async Task<ActionResult> List(string category, int page = 1, int pageSize = 10)
        {
            var results = await patentsForClassificationQuery.ExecuteAsync(category, page, pageSize);

            return this.JsonNetResult(results, JsonRequestBehavior.AllowGet);
        }

        private static int x;
        [Route("category/add/{*category}")]
        public async Task<ActionResult> Add(string category)
        {
            using (var session = documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(new PatentableEntity {Category = category, Name = x++.ToString(), Description = "Tis is a description of " + x, Owner = "spadger", Attachments = GetAttachments()});
                await session.SaveChangesAsync();
            }

            return Content("Stored: " + category);
        }

        static Random r = new Random();
        private List<Attachment> GetAttachments()
        {
            var count = r.Next(0, 3);

            var results = new List<Attachment>();

            for (int i = 0; i < count; i++)
            {
                results.Add(new Attachment {Name = Guid.NewGuid().ToString()});
            }

            return results;
        }
    }
}