using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PatentSpoiler.App.Attachments
{
    public interface IStagedAttachmentAdapter
    {
        Task SaveAsync(Guid id, Stream source, string contentType);
        Task<string> GetAsync(Guid id, Stream destination);
    }

    public class StagedAttachmentAdapter : IStagedAttachmentAdapter
    {
        private readonly CloudBlobContainer cloudBlobContainer;

        public StagedAttachmentAdapter(CloudBlobClient cloudBlobClient)
        {
            cloudBlobContainer = cloudBlobClient.GetContainerReference("attachments");
        }

        public async Task SaveAsync(Guid id, Stream source, string contentType)
        {
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(id.ToString());
            blockBlob.Properties.ContentType = contentType;
            await blockBlob.UploadFromStreamAsync(source);
        }

        public async Task<string> GetAsync(Guid id, Stream destination)
        {
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(id.ToString());
            await blockBlob.DownloadToStreamAsync(destination);
            return blockBlob.Properties.ContentType;
        }
    }
}