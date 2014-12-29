using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PatentSpoiler.App;
using PatentSpoiler.App.Attachments;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.DTOs.Item;
using PatentSpoiler.App.Security;

namespace PatentSpoiler.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly IStagingAttachmentAdapter stagingAttachmentAdapter;
        private readonly IStagedAttachmentAdapter stagedAttachmentAdapter;
        private readonly HttpContextBase context;

        public AttachmentController(IStagingAttachmentAdapter stagingAttachmentAdapter, IStagedAttachmentAdapter stagedAttachmentAdapter, HttpContextBase context)
        {
            this.stagingAttachmentAdapter = stagingAttachmentAdapter;
            this.stagedAttachmentAdapter = stagedAttachmentAdapter;
            this.context = context;
        }

        [HttpGet]
        [Route("attachment/{id:guid}", Name = "GetAttachment")]
        [AuthoriseRoles(UserRole.VerifiedMember)]
        public async Task<ActionResult> Get(Guid id)
        {
            var attachmentDescriptor = await stagedAttachmentAdapter.GetAsync(id, context.Response.OutputStream);
            Response.ContentType = attachmentDescriptor.ContentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + attachmentDescriptor.FileName);
            return new EmptyResult();
        }

        [HttpPost]
        [Route("attachment")]
        [AuthoriseRoles(UserRole.VerifiedMember)]
        public async Task<ActionResult> Create()
        {
            var files = context.Request.Files;
            if (files.Count == 0)
            {
                return new HttpStatusCodeResult(400, "No file uploaded");
            }

            var descriptor = files[0];

            var id = await stagingAttachmentAdapter.SaveAsync(descriptor.InputStream);

            var result = new AttachmentViewModel
            {
                Id = id,
                Type = descriptor.ContentType,
                Name = descriptor.FileName,
                Size = descriptor.ContentLength
            };
            return new JsonNetResult{Data = result};
        }
    }
}