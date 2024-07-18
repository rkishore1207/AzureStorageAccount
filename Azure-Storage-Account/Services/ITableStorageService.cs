using Azure_Storage_Account.Data;

namespace Azure_Storage_Account.Services
{
    public interface ITableStorageService
    {
        Task DeleteAttendee(string industry, string id);
        Task<AttendeeEntity> GetAttendee(string industry, string id);
        List<AttendeeEntity> GetAttendees();
        Task UpsertAttendee(AttendeeEntity attendeeEntity);
    }
}