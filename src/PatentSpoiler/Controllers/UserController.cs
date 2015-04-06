using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App;
using PatentSpoiler.App.Data.Queries.PatentableEntities;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.Security;
using Raven.Client;

namespace PatentSpoiler.Controllers
{
    public class UserController : Controller
    {
        private readonly IAsyncDocumentSession session;
        private readonly IRetrievePatentsByOwnerQuery retrievePatentsByOwnerQuery;
        private readonly PatentSpoilerUser user;

        public UserController(IAsyncDocumentSession session, IRetrievePatentsByOwnerQuery retrievePatentsByOwnerQuery, PatentSpoilerUser user)
        {
            this.session = session;
            this.retrievePatentsByOwnerQuery = retrievePatentsByOwnerQuery;
            this.user = user;
        }

        [Route("me")]
        [AuthoriseRoles(UserRole.EmailConfirmed)]
        public Task<ActionResult> Me()
        {
            return Index(user.Id);
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
            return View("Index");
        }

        [Route("user/{username}/patents")]
        public async Task<ActionResult> GetPatentsForUser(string username, int skip = 1, int pageSize = 20)
        {
            var results = await retrievePatentsByOwnerQuery.ExecuteAsync(username, skip, pageSize);

            return this.JsonNetResult(results, JsonRequestBehavior.AllowGet);
        }
    }
}