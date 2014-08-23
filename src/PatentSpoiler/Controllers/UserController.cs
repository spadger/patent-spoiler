using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App;
using PatentSpoiler.App.Data.Queries.PatentableEntities;
using PatentSpoiler.App.Domain.Security;
using Raven.Client;

namespace PatentSpoiler.Controllers
{
    public class UserController : Controller
    {
        private readonly IAsyncDocumentSession session;
        private readonly IRetrievePatentsByOwnerQuery retrievePatentsByOwnerQuery;

        public UserController(IAsyncDocumentSession session, IRetrievePatentsByOwnerQuery retrievePatentsByOwnerQuery)
        {
            this.session = session;
            this.retrievePatentsByOwnerQuery = retrievePatentsByOwnerQuery;
        }

        [Route("user/{username}")]
        public async Task<ActionResult> Index(string username)
        {
            var user = await session.LoadAsync<PatentSpoilerUser>(username);
            if (user == null)
            {
                return new HttpNotFoundResult("User could no be found");
            }
            ViewBag.UserId = username;
            return View();
        }

        [Route("user/{username}/patents")]
        public async Task<ActionResult> List(string username, int page = 1, int pageSize = 10)
        {
            var results = await retrievePatentsByOwnerQuery.ExecuteAsync(username, page, pageSize);

            return this.JsonNetResult(results, JsonRequestBehavior.AllowGet);
        }
    }
}