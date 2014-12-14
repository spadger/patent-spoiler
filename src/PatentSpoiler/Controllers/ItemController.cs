using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App.Commands.PatentableEntities;
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
        private readonly UpdatePatentableEntityCommand updatePatentableEntityCommand;
        private readonly PatentSpoilerUser user;
        private readonly IGetPatentableEntityForDisplayQuery getPatentableEntityForDisplayQuery;
        private readonly IGetPatentableEntityForEditQuery getPatentableEntityForEditQuery;

        public ItemController(SaveNewPatentableEntityCommand saveNewPatentableEntityCommand,
            UpdatePatentableEntityCommand updatePatentableEntityCommand, PatentSpoilerUser user,
            IGetPatentableEntityForDisplayQuery getPatentableEntityForDisplayQuery,
            IGetPatentableEntityForEditQuery getPatentableEntityForEditQuery)
        {
            this.saveNewPatentableEntityCommand = saveNewPatentableEntityCommand;
            this.updatePatentableEntityCommand = updatePatentableEntityCommand;
            this.user = user;
            this.getPatentableEntityForDisplayQuery = getPatentableEntityForDisplayQuery;
            this.getPatentableEntityForEditQuery = getPatentableEntityForEditQuery;
        }

        [HttpGet]
        [AuthoriseRoles(UserRole.PaidMember)]
        [PatentCategoryMustExist("category", IsOptional = true)]
        [Route("item/add/{*category}")]
        public ActionResult Add(string category)
        {
            return View(new AddItemDisplayViewModel {Category = category});
        }

        [HttpPost]
        [AuthoriseRoles(UserRole.PaidMember)]
        [Route("item/add/{*category}")]
        public async Task<ActionResult> Add(AddItemRequestViewModel item)
        {
            var id = await saveNewPatentableEntityCommand.SaveAsync(item, User.Identity.Name);
            return Json(new {id});
        }

        [HttpGet]
        [AuthoriseRoles(UserRole.PaidMember)]
        [Route("item/{id}")]
        public async Task<ActionResult> View(int id)
        {
            var item = await getPatentableEntityForDisplayQuery.ExecuteAsync(id);
            return View(item);
        }

        [HttpGet]
        [AuthoriseRoles(UserRole.PaidMember)]
        [Route("item/{id}/edit")]
        public async Task<ActionResult> Edit(int id)
        {
            var item = await getPatentableEntityForEditQuery.ExecuteAsync(id);

            if (user.Id != item.Owner)
            {
                return new RedirectResult("/item/" + id);
            }

            return View(item);
        }

        [HttpPut]
        [AuthoriseRoles(UserRole.PaidMember)]
        [Route("item/{id}")]
        public async Task<ActionResult> Edit(UpdateItemRequestViewModel item)
        {
            await updatePatentableEntityCommand.UpdateAsync(item, user.Id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}