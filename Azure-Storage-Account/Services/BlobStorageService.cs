using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace Azure_Storage_Account.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private string ContainerName = "attendeeimages";
        private async Task<BlobContainerClient> GetBlobContainerClient()
        {

            BlobContainerClient blobContainer = new BlobContainerClient(Constant.Constant.StorageConnectionString, ContainerName);
            await blobContainer.CreateIfNotExistsAsync();
            return blobContainer;
        }

        public async Task<string> UploadBlob(IFormFile formFile, string imageName, string originalBlobName)
        {
            var blobName = $"{imageName}-{Path.GetFileName(formFile.FileName)}";
            var blobContainer = await GetBlobContainerClient();
            if(!string.IsNullOrEmpty(originalBlobName))
            {
                await DeleteBlob(imageName);
            }
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                memoryStream.Position = 0;
                var blob = blobContainer.GetBlobClient(blobName);
                var client = blob.UploadAsync(memoryStream,overwrite: true);
            }
            return blobName;
        }

        public async Task<string> GetBlobUrl(string imageName)
        {
            var blobContainer = await GetBlobContainerClient();
            var blob = blobContainer.GetBlobClient(imageName);
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobContainer.Name,
                BlobName = blob.Name,
                ExpiresOn = DateTime.UtcNow.AddMinutes(2),
                Protocol = SasProtocol.Https,
                Resource = "b"
            };
            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);
            return blob.GenerateSasUri(blobSasBuilder).ToString();
        }

        public async Task DeleteBlob(string imageName)
        {
            var blobContainer = await GetBlobContainerClient();
            await blobContainer.DeleteBlobIfExistsAsync(imageName);
        }
    }
}
