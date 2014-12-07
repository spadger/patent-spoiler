using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PatentSpoiler.App.Attachments
{

    public interface IStagedAttachmentAdapter
    {
        Task SaveAsync(Guid id, Stream source);
        Task GetAsync(Guid id, Stream destination);
        Task DeleteAsync(Guid id);
    }

    public class StagedAttachmentAdapter : IStagedAttachmentAdapter
    {
        private readonly CloudBlobClient cloudBlobClient;
        private readonly CloudBlobContainer cloudBlobContainer;

        public StagedAttachmentAdapter(CloudBlobClient cloudBlobClient)
        {
            this.cloudBlobClient = cloudBlobClient;
            cloudBlobContainer = cloudBlobClient.GetContainerReference("attachments");
        }

        public async Task SaveAsync(Guid id, Stream source)
        {
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(id.ToString());
            await blockBlob.UploadFromStreamAsync(source);
            
        }

        public async Task GetAsync(Guid id, Stream destination)
        {
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(id.ToString());
            await blockBlob.DownloadToStreamAsync(destination);
        }

        public async Task DeleteAsync(Guid id)
        {
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(id.ToString());
            await blockBlob.DeleteIfExistsAsync();
        }
    }
}