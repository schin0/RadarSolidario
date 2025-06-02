using Azure;
using Azure.Data.Tables;

namespace Core.Entities;

public class Info : ITableEntity
{
    public string PartitionKey { get; set; } = "Info";
    public string RowKey { get; set; } = "1";
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public int AmountHelp { get; set; }
    public int QuantityVolunteers { get; set; }
    public int Connections { get; set; }
    public int CommunitiesServed { get; set; }
}