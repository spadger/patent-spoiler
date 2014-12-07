using System;
using System.IO;
using System.Threading.Tasks;

namespace PatentSpoiler.App.Attachments
{
    public interface IStagingAttachmentAdapter
    {
        Task<Guid> Save(byte[] data);
        void Delete(Guid id);
    }

    public class StagingAttachmentAdapter : IStagingAttachmentAdapter
    {
        private IAttachmentStagingSettings attachmentStagingSettings;

        public StagingAttachmentAdapter(IAttachmentStagingSettings attachmentStagingSettings)
        {
            this.attachmentStagingSettings = attachmentStagingSettings;
        }

        public async Task<Guid> Save(byte[] data)
        {
            var fileId = Guid.NewGuid();
            var path = Path.Combine(attachmentStagingSettings.Path, fileId + ".dat");

            using (var sourceStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None, 8192, true))
            {
                await sourceStream.WriteAsync(data, 0, data.Length);
            };

            return fileId;
        }

        public void Delete(Guid id)
        {
            var path = Path.Combine(attachmentStagingSettings.Path, id + ".dat");

            File.Delete(path);
        }
    }
}