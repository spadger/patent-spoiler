using System.Web.Mvc;
using PatentSpoiler.App.Commands;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.DTOs;
using PatentSpoiler.App.Filters;
using PatentSpoiler.App.Security;

namespace PatentSpoiler.Controllers
{
    public class ItemController : Controller
    {
        private readonly SaveNewPatentableEntityCommand saveNewPatentableEntityCommand;
        private readonly PatentSpoilerUser user;

        public ItemController(SaveNewPatentableEntityCommand saveNewPatentableEntityCommand, PatentSpoilerUser user)
        {
            this.saveNewPatentableEntityCommand = saveNewPatentableEntityCommand;
            this.user = user;
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
            return Json(new{za="Hello"});
        }
    }
}