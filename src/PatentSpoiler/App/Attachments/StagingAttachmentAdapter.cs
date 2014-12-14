using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace PatentSpoiler.App.Attachments
{
    public interface IStagingAttachmentAdapter
    {
        Task<Guid> SaveAsync(Stream inputStream);
        FileStream Get(Guid id);
        void Delete(Guid id);
    }

    public class StagingAttachmentAdapter : IStagingAttachmentAdapter
    {
        private IAttachmentStagingSettings attachmentStagingSettings;

        public StagingAttachmentAdapter(IAttachmentStagingSettings attachmentStagingSettings)
        {
            this.attachmentStagingSettings = attachmentStagingSettings;
        }

        public async Task<Guid> SaveAsync(Stream inputStream)
        {
            var id = Guid.NewGuid();
            var path = GetPath(id);

            path = HttpContext.Current.Request.MapPath(path);
            using (var destinationStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None, 8192, true))
            {
                await inputStream.CopyToAsync(destinationStream);
            };

            return id;
        }

        public FileStream Get(Guid id)
        {
            var path = GetPath(id);

            return File.OpenRead(path);
        }

        public void Delete(Guid id)
        {
            var path = GetPath(id);

            File.Delete(path);
        }

        private string GetPath(Guid id)
        {
            return Path.Combine(attachmentStagingSettings.Path, id + ".dat");
        }
    }
}