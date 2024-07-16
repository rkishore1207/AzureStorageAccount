using Azure;
using Azure.Data.Tables;

namespace Azure_Storage_Account.Data
{
    public class AttendeeEntity : ITableEntity
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set; }
        public string? Industry { get; set; } // Our own Partition key, this should be present in all Models
        public string? PartitionKey {  get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
