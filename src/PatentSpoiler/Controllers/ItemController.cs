using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App.Commands;
using PatentSpoiler.App.Data.Queries.PatentableEntities;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.DTOs.Item;
using PatentSpoiler.App.Filters;
using PatentSpoiler.App.Security;

namespace PatentSpoiler.Controllers
{
    public class ItemController : Controller
    {
        private readonly SaveNewPatentableEntityCommand saveNewPatentableEntityCommand;
        private readonly PatentSpoilerUser user;
        private readonly IGetPatentableEntityForDisplayQuery getPatentableEntityForDisplayQuery;

        public ItemController(SaveNewPatentableEntityCommand saveNewPatentableEntityCommand, PatentSpoilerUser user, IGetPatentableEntityForDisplayQuery getPatentableEntityForDisplayQuery)
        {
            this.saveNewPatentableEntityCommand = saveNewPatentableEntityCommand;
            this.user = user;
            this.getPatentableEntityForDisplayQuery = getPatentableEntityForDisplayQuery;
        }

        [HttpGet]
        [AuthoriseRoles(UserRole.PaidMember)]
        [PatentCategoryMustExist("category", IsOptional = true)]
        [Route("item/add/{*category}")]
        public ActionResult Add(string category)
        {
            return View(new AddItemDisplayViewModel{Category = category});
        }

        [HttpPost]
        [AuthoriseRoles(UserRole.PaidMember)]
        [Route("item/add/{*category}")]
        public ActionResult Add(AddItemRequestViewModel item)
        {
            var user = User.Identity;
            saveNewPatentableEntityCommand.Save(item, user.Name);
            return Json(new { za = "Hello" });
        }

        [HttpGet]
        [AuthoriseRoles(UserRole.PaidMember)]
        [Route("item/{id}")]
        public async Task<ActionResult> View(int id)
        {
            var item = await getPatentableEntityForDisplayQuery.ExecuteAsync(id);
            return View(item);
        }

        [HttpPost]
        [AuthoriseRoles(UserRole.PaidMember)]
        [Route("item/{id}/edit")]
        public async Task<ActionResult> Edit(int id)
        {
            var item = await getPatentableEntityForDisplayQuery.ExecuteAsync(id);
            return View(item);
        }
    }
}