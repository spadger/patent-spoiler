using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using PatentSpoiler.App.Attachments;

namespace PatentSpoiler.Controllers
{
    public class StorageTestController : Controller
    {
        private readonly IStagedAttachmentAdapter stagedAttachmentAdapter;

        public StorageTestController(IStagedAttachmentAdapter stagedAttachmentAdapter)
        {
            this.stagedAttachmentAdapter = stagedAttachmentAdapter;
        }

        public async Task<ActionResult> A()
        {
            using (var fs = new FileStream(@"d:\test.png", FileMode.Open))
            {
                await stagedAttachmentAdapter.SaveAsync(id, fs, "application/octet-stream");
            }
            return Content("Saved");
        }

        public async Task<ActionResult> B()
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=test.png");
            Response.ContentType = "application/octet-stream";
            
            await stagedAttachmentAdapter.GetAsync(id, Response.OutputStream);
            return new EmptyResult();
        }

        private static Guid id = Guid.NewGuid();

    }
}