using Azure;
using Core.Mappers;
using Infra.Entities;

namespace UnitTests.Mappers;

public class InfoMapperTests
{
    [Fact]
    public void ToDto_MapsAllPropertiesCorrectly()
    {
        var entity = new Info
        {
            QuantityHelp = 7,
            QuantityVolunteers = 3,
            Connections = 12,
            CommunitiesServed = 5,
            ETag = new ETag("test-etag"),
            PartitionKey = "TestPartition",
            RowKey = "TestRow",
            Timestamp = DateTimeOffset.UtcNow
        };

        var dto = entity.ToDto();

        Assert.Equal(entity.QuantityHelp, dto.QuantityHelp);
        Assert.Equal(entity.QuantityVolunteers, dto.QuantityVolunteers);
        Assert.Equal(entity.Connections, dto.Connections);
        Assert.Equal(entity.CommunitiesServed, dto.CommunitiesServed);
    }
}
