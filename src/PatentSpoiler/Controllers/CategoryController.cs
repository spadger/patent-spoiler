using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App;
using PatentSpoiler.App.Data.Queries.PatentableEntities;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs;
using PatentSpoiler.App.Filters;
using Raven.Client;

namespace PatentSpoiler.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRetrievePatentsByClassificationQuery patentsForClassificationQuery;
        private readonly IAsyncDocumentSession session;

        public CategoryController(IRetrievePatentsByClassificationQuery patentsForClassificationQuery, IAsyncDocumentSession session)
        {
            this.patentsForClassificationQuery = patentsForClassificationQuery;
            this.session = session;
        }

        [Route("category/{*category}")]
        [PatentCategoryMustExist("category")]
        public async Task<ActionResult> Index(string category)
        {
            return View(new CategoryListDisplayViewModel{Category = category});
        }

        [Route("category/list/{*category}")]
        [PatentCategoryMustExist("category")]
        public async Task<ActionResult> List(string category, int page = 1, int pageSize = 10)
        {
            var results = await patentsForClassificationQuery.ExecuteAsync(category, page, pageSize);

            return this.JsonNetResult(results, JsonRequestBehavior.AllowGet);
        }

        private static int x;
        [Route("category/add/{*category}")]
        [PatentCategoryMustExist("category")]
        public async Task<ActionResult> Add(string category)
        {
            await session.StoreAsync(new PatentableEntity
            {
                Categories = new HashSet<string>(new[]{category}), 
                Name = x++.ToString(), 
                Description = "Tis is a description of " + x,
                Owner = "spadger",
                Attachments = GetAttachments()
            });
            await session.SaveChangesAsync();

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