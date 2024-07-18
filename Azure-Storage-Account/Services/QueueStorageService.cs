using Azure.Storage.Queues;
using Azure_Storage_Account.Models;
using Newtonsoft.Json;

namespace Azure_Storage_Account.Services
{
    public class QueueStorageService : IQueueStorageService
    {
        private const string QueueName = "attendee-queue";
        private readonly QueueClient _queueClient;

        public QueueStorageService(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task SendMail(EmailMessage emailMessage)
        {
            //QueueClient queueClient = new QueueClient(Constant.Constant.StorageConnectionString, QueueName, new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 });
            //await queueClient.CreateIfNotExistsAsync();
            var message = JsonConvert.SerializeObject(emailMessage);
            await _queueClient.SendMessageAsync(message);
        }
    }
}
