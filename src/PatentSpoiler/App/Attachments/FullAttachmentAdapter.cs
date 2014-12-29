using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PatentSpoiler.App.Attachments
{
    public interface IStagedAttachmentAdapter
    {
        Task SaveAsync(Guid id, Stream source, string contentType, string fileName);
        Task<StagedFileDescritor> GetAsync(Guid id, Stream destination);
    }

    public class StagedAttachmentAdapter : IStagedAttachmentAdapter
    {
        private const string original_file_name_metadata_key = "OriginalFileName";

        private readonly CloudBlobContainer cloudBlobContainer;

        public StagedAttachmentAdapter(CloudBlobClient cloudBlobClient)
        {
            cloudBlobContainer = cloudBlobClient.GetContainerReference("attachments");
        }

        public async Task SaveAsync(Guid id, Stream source, string contentType, string fileName)
        {
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(id.ToString());
            blockBlob.Properties.ContentType = contentType;
            blockBlob.Metadata.Add(original_file_name_metadata_key, fileName);
            await blockBlob.UploadFromStreamAsync(source);
        }

        public async Task<StagedFileDescritor> GetAsync(Guid id, Stream destination)
        {
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(id.ToString());
            await blockBlob.DownloadToStreamAsync(destination);
            return new StagedFileDescritor
            {
                ContentType = blockBlob.Properties.ContentType,
                FileName = blockBlob.Metadata[original_file_name_metadata_key]
            };
        }
    }

    public class StagedFileDescritor
    {
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}