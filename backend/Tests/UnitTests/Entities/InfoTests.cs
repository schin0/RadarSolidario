using Azure;
using Core.Entities;

namespace UnitTests.Entities;

public class InfoTests
{
    [Fact]
    public void Info_DefaultConstructor_SetsDefaultValues()
    {
        var info = new Info();

        Assert.Equal("Info", info.PartitionKey);
        Assert.Equal("1", info.RowKey);
        Assert.Null(info.Timestamp);
        Assert.Equal(default, info.ETag);
        Assert.Equal(0, info.AmountHelp);
        Assert.Equal(0, info.QuantityVolunteers);
        Assert.Equal(0, info.Connections);
        Assert.Equal(0, info.CommunitiesServed);
    }

    [Fact]
    public void Info_SetProperties_PropertiesAreSet()
    {
        var info = new Info
        {
            PartitionKey = "TestPartition",
            RowKey = "TestRow",
            Timestamp = DateTimeOffset.UtcNow,
            ETag = new ETag("test"),
            AmountHelp = 10,
            QuantityVolunteers = 5,
            Connections = 3,
            CommunitiesServed = 2
        };

        Assert.Equal("TestPartition", info.PartitionKey);
        Assert.Equal("TestRow", info.RowKey);
        Assert.NotNull(info.Timestamp);
        Assert.Equal(new ETag("test"), info.ETag);
        Assert.Equal(10, info.AmountHelp);
        Assert.Equal(5, info.QuantityVolunteers);
        Assert.Equal(3, info.Connections);
        Assert.Equal(2, info.CommunitiesServed);
    }
}
