using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PatentSpoiler.App;
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
        private readonly IGetPatentsBySetIdQuery getPatentsBySetIdQuery;

        public ItemController(SaveNewPatentableEntityCommand saveNewPatentableEntityCommand,
            UpdatePatentableEntityCommand updatePatentableEntityCommand, PatentSpoilerUser user,
            IGetPatentableEntityForDisplayQuery getPatentableEntityForDisplayQuery,
            IGetPatentableEntityForEditQuery getPatentableEntityForEditQuery,
            IGetPatentsBySetIdQuery getPatentsBySetIdQuery)
        {
            this.saveNewPatentableEntityCommand = saveNewPatentableEntityCommand;
            this.updatePatentableEntityCommand = updatePatentableEntityCommand;
            this.user = user;
            this.getPatentableEntityForDisplayQuery = getPatentableEntityForDisplayQuery;
            this.getPatentableEntityForEditQuery = getPatentableEntityForEditQuery;
            this.getPatentsBySetIdQuery = getPatentsBySetIdQuery;
        }

        [HttpGet]
        [AuthoriseRoles(UserRole.EmailConfirmed)]
        [PatentCategoryMustExist("category", IsOptional = true)]
        [Route("item/add/{*category}")]
        public ActionResult Add(string category)
        {
            return View(new AddItemDisplayViewModel {Category = category});
        }

        [HttpPost]
        [AuthoriseRoles(UserRole.EmailConfirmed)]
        [Route("item/add/{*category}")]
        public async Task<ActionResult> Add(AddItemRequestViewModel item)
        {
            var id = await saveNewPatentableEntityCommand.SaveAsync(item, User.Identity.Name);
            return Json(new {id});
        }

        [HttpGet]
        [Route("item/{id}")]
        public async Task<ActionResult> View(int id)
        {
            var item = await getPatentableEntityForDisplayQuery.ExecuteAsync(id);

	        if (item == null)
	        {
		        throw new HttpException(404, "Item not found");
	        }

            ViewBag.CanEdit = item.Owner == (user==null ? null :user.Id);
            return View(item);
        }

        [HttpGet]
        [AuthoriseRoles(UserRole.EmailConfirmed)]
        [Route("item/{id}/edit", Name = "EditItem")]
        public async Task<ActionResult> Edit(int id)
        {
            var item = await getPatentableEntityForEditQuery.ExecuteAsync(id);

            if (item == null)
            {
                return new HttpStatusCodeResult(404);
            }

            if (user.Id != item.Owner)
            {
                return new RedirectResult("/item/" + id);
            }

            if (item.Archived)
            {
                return RedirectToAction("View");
            }

            return View(item);
        }

        [HttpPut]
        [AuthoriseRoles(UserRole.EmailConfirmed)]
        [Route("item/{id}")]
        public async Task<ActionResult> Edit(UpdateItemRequestViewModel item)
        {
            await updatePatentableEntityCommand.UpdateAsync(item, user.Id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("item/{setId:guid}/versions")]
        public async Task<ActionResult> PreviousVersions(Guid setId, int skip, int take)
        {
            var previousVersions = await getPatentsBySetIdQuery.ExecuteAsync(setId, skip, take);

            return this.JsonNetResult(previousVersions, JsonRequestBehavior.AllowGet);
        }
    }
}