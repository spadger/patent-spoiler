using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PatentSpoiler.App.Attachments;

namespace PatentSpoiler.Controllers
{
    public class StorageTestController : Controller
    {
        private IStagedAttachmentAdapter stagedAttachmentAdapter;
        private readonly HttpContextBase context;

        public StorageTestController(IStagedAttachmentAdapter stagedAttachmentAdapter, HttpContextBase context)
        {
            this.stagedAttachmentAdapter = stagedAttachmentAdapter;
            this.context = context;
        }

        public async Task<ActionResult> A()
        {
            await stagedAttachmentAdapter.SaveAsync(id, new FileStream(@"d:\test.png", FileMode.Open));
            return Content("Saved");
        }

        public async Task<ActionResult> B()
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=result.png");
            await stagedAttachmentAdapter.GetAsync(id, context.Response.OutputStream);
            return new EmptyResult();
        }

        private static Guid id = Guid.NewGuid();

    }
}