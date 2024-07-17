using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text;

namespace QueueConsumer
{
    internal class Program
    {
        private readonly static string? ConnectionString = Environment.GetEnvironmentVariable("StorageAccount");

        private static async Task<string> RetrieveNextMessage(QueueClient queueClient)
        {
            QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(1);
            var data = Convert.FromBase64String(messages[0].Body.ToString());
            string theMessage = Encoding.UTF8.GetString(data);
            await queueClient.DeleteMessageAsync(messages[0].MessageId, messages[0].PopReceipt);
            return theMessage;
        }

        private static async Task QueueServiceAsync()
        {
            QueueClient queueClient = new QueueClient(ConnectionString, "attendee-queue");
            if (await queueClient.ExistsAsync())
            {
                QueueProperties queueProperties = await queueClient.GetPropertiesAsync();
                for(int i = 0; i < queueProperties.ApproximateMessagesCount; i++)
                {
                    string message = await RetrieveNextMessage(queueClient);
                    Console.WriteLine(message);
                }
            }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            await QueueServiceAsync();
        }
    }
}
