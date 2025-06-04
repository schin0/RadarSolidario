using Api.Controllers;
using Core.DTOs;
using Core.Mappers;
using Infra.Entities;
using Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Controllers;

public class InfoControllerTests
{
    [Fact]
    public async Task Get_ReturnsOk_WhenInfoExists()
    {
        var info = new Info
        {
            PartitionKey = "pk",
            RowKey = "rk",
            QuantityHelp = 10,
            QuantityVolunteers = 5,
            Connections = 2,
            CommunitiesServed = 1
        };
        var infoDto = info.ToDto();

        var repoMock = new Mock<IInfoRepository>();
        repoMock.Setup(r => r.GetAsync()).ReturnsAsync(info);

        var controller = new InfoController(repoMock.Object);

        var result = await controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDto = Assert.IsType<InfoDto>(okResult.Value);
        Assert.Equal(infoDto.QuantityHelp, returnedDto.QuantityHelp);
        Assert.Equal(infoDto.QuantityVolunteers, returnedDto.QuantityVolunteers);
        Assert.Equal(infoDto.Connections, returnedDto.Connections);
        Assert.Equal(infoDto.CommunitiesServed, returnedDto.CommunitiesServed);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenInfoIsNull()
    {
        var repoMock = new Mock<IInfoRepository>();
        repoMock.Setup(r => r.GetAsync()).ReturnsAsync((Info?)null);

        var controller = new InfoController(repoMock.Object);

        var result = await controller.Get();

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
