using Azure_Storage_Account.Models;

namespace Azure_Storage_Account.Services
{
    public interface IQueueStorageService
    {
        Task SendMail(EmailMessage emailMessage);
    }
}