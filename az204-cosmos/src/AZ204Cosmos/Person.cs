using Azure;
using Azure.Data.Tables;

namespace AZ204Cosmos
{
    public class Person : ITableEntity
    {
        public string PartitionKey { get; set; } = Guid.NewGuid().ToString();
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public ETag ETag { get; set; } = ETag.All;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        // TODO: Later
        // public DateTime DateOfBirth { get; set; }
        // public bool IsNice { get; set; }
        // public decimal Salary { get; set; }
    }
}

