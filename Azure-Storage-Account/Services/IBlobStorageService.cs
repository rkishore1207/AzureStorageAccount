
namespace Azure_Storage_Account.Services
{
    public interface IBlobStorageService
    {
        Task DeleteBlob(string imageName);
        Task<string> GetBlobUrl(string imageName);
        Task<string> UploadBlob(IFormFile formFile, string imageName, string originalBlobName);
    }
}