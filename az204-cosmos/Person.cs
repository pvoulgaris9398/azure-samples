using Azure;
using Azure.Data.Tables;

public class Person : ITableEntity
{
    private string _partitionKey = Guid.NewGuid().ToString();
    private string _rowKey = Guid.NewGuid().ToString();
    private DateTimeOffset? _timeStamp = DateTimeOffset.UtcNow;
    private ETag _eTag = ETag.All;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    public string PartitionKey { get => _partitionKey; set => _partitionKey = value; }
    public string RowKey { get => _rowKey; set => _rowKey = value; }
    public DateTimeOffset? Timestamp { get => _timeStamp; set => _timeStamp = value; }
    public ETag ETag { get => _eTag; set => _eTag = value; }

    public string FirstName { get => _firstName; set => _firstName = value; }
    public string LastName { get => _lastName; set => _lastName = value; }
    // TODO: Later
    // public DateTime DateOfBirth { get; set; }
    // public bool IsNice { get; set; }
    // public decimal Salary { get; set; }
}
