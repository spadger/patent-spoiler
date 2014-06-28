using System.Web.Mvc;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.DTOs;
using PatentSpoiler.App.Security;

namespace PatentSpoiler.Controllers
{
    public class ItemController : Controller
    {
        [HttpGet]
        [AuthoriseRoles(UserRole.PaidMember)]
        [Route("item/add/{*category}")]
        public ActionResult Add(string category)
        {
            return View(new AddItemDisplayViewModel{Category = category});
        }

        [HttpPost]
        [AuthoriseRoles(UserRole.PaidMember)]
        [Route("item/add/{*category}")]
        public ActionResult Add(string category, AddItemRequestViewModel item)
        {
            return Json(new{za="Hello"});
        }
    }
}