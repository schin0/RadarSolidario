using Core.DTOs;

namespace UnitTests.Dto;

public class InfoDtoTests
{
    [Fact]
    public void Constructor_Should_Set_All_Properties()
    {
        var dto = new InfoDto(1, 2, 3, 4);

        Assert.Equal(1, dto.QuantityHelp);
        Assert.Equal(2, dto.QuantityVolunteers);
        Assert.Equal(3, dto.Connections);
        Assert.Equal(4, dto.CommunitiesServed);
    }

    [Fact]
    public void Records_With_Same_Values_Should_Be_Equal()
    {
        var dto1 = new InfoDto(5, 6, 7, 8);
        var dto2 = new InfoDto(5, 6, 7, 8);

        Assert.Equal(dto1, dto2);
        Assert.True(dto1 == dto2);
    }

    [Fact]
    public void Records_With_Different_Values_Should_Not_Be_Equal()
    {
        var dto1 = new InfoDto(1, 2, 3, 4);
        var dto2 = new InfoDto(9, 8, 7, 6);

        Assert.NotEqual(dto1, dto2);
        Assert.False(dto1 == dto2);
    }

    [Fact]
    public void Can_Deconstruct_InfoDto()
    {
        var dto = new InfoDto(10, 20, 30, 40);
        var (help, volunteers, connections, communities) = dto;

        Assert.Equal(10, help);
        Assert.Equal(20, volunteers);
        Assert.Equal(30, connections);
        Assert.Equal(40, communities);
    }
}
