using Azure;
using Azure.Data.Tables;
using Azure_Storage_Account.Data;

namespace Azure_Storage_Account.Services
{
    public class TableStorageService : ITableStorageService
    {
        private readonly IConfiguration _configuration;
        private const string TABLE = "Attendees";
        private string? StorageConnectionString = Environment.GetEnvironmentVariable("StorageAccount");
        public TableStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<TableClient> GetTableClient()
        {
            var tableServiceClient = new TableServiceClient(StorageConnectionString);
            var tableClient = tableServiceClient.GetTableClient(TABLE);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }

        public async Task<AttendeeEntity> GetAttendee(string industry, string id) //Partition Key is required to fetch or delete Entity
        {
            var tableClient = await GetTableClient();
            return await tableClient.GetEntityAsync<AttendeeEntity>(industry, id);
        }

        public async Task<List<AttendeeEntity>> GetAttendees()
        {
            var tableClient = await GetTableClient();
            Pageable<AttendeeEntity> attendees = tableClient.Query<AttendeeEntity>();
            return attendees.ToList();
        }

        public async Task UpsertAttendee(AttendeeEntity attendeeEntity)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(attendeeEntity);
        }

        public async Task DeleteAttendee(string industry, string id)
        {
            var tableClient = await GetTableClient();
            await tableClient.DeleteEntityAsync(industry, id);
        }
    }
}
