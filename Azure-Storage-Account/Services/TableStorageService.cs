using Azure;
using Azure.Data.Tables;
using Azure_Storage_Account.Data;

namespace Azure_Storage_Account.Services
{
    public class TableStorageService : ITableStorageService
    {
        private const string TABLE = "Attendees";
        private readonly TableClient _tableClient;

        public TableStorageService(TableClient tableClient)
        {
            _tableClient = tableClient;
        }

        private async Task<TableClient> GetTableClient()
        {
            var tableServiceClient = new TableServiceClient(Constant.Constant.StorageConnectionString);
            var tableClient = tableServiceClient.GetTableClient(TABLE);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }

        public async Task<AttendeeEntity> GetAttendee(string industry, string id) //Partition Key is required to fetch or delete Entity
        {
            //var tableClient = await GetTableClient();
            return await _tableClient.GetEntityAsync<AttendeeEntity>(industry, id);
        }

        public List<AttendeeEntity> GetAttendees()
        {
            //var tableClient = await GetTableClient();
            Pageable<AttendeeEntity> attendees = _tableClient.Query<AttendeeEntity>();
            return attendees.OrderBy(attendee => attendee.FirstName).ToList();
        }

        public async Task UpsertAttendee(AttendeeEntity attendeeEntity)
        {
            //var tableClient = await GetTableClient();
            await _tableClient.UpsertEntityAsync(attendeeEntity);
        }

        public async Task DeleteAttendee(string industry, string id)
        {
            //var tableClient = await GetTableClient();
            await _tableClient.DeleteEntityAsync(industry, id);
        }
    }
}
