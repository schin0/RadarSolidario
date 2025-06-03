using Core.Request;
using Core.UseCase;
using Infra.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.UseCase;

public class ProcessWhatsappMessageTest
{
    private readonly Mock<ILogger<ProcessWhatsappMessage>> _loggerMock;
    private readonly Mock<IInfoRepository> _infoRepositoryMock;
    private readonly ProcessWhatsappMessage _handler;

    public ProcessWhatsappMessageTest()
    {
        _loggerMock = new Mock<ILogger<ProcessWhatsappMessage>>();
        _infoRepositoryMock = new Mock<IInfoRepository>();
        _handler = new ProcessWhatsappMessage(_loggerMock.Object, _infoRepositoryMock.Object);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("preciso de ajuda")]
    [InlineData("preciso")]
    [InlineData("🆘")]
    public async Task Handle_ShouldRespondHelpMessage_AndIncrementHelp(string input)
    {
        var request = new ProcessWhatsappMessageRequest("user1", input, "newWhatsappMessage", 1748909787, "false_5511999999999@c.us_3F27B518730499998888");

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Contains("Seu pedido de ajuda foi registrado", response);
        _infoRepositoryMock.Verify(r => r.IncrementQuantityHelpAsync(), Times.Once);
    }

    [Theory]
    [InlineData("2")]
    [InlineData("posso ajudar")]
    [InlineData("sou voluntario")]
    [InlineData("sou voluntária")]
    [InlineData("voluntario")]
    [InlineData("voluntária")]
    [InlineData("�")]
    public async Task Handle_ShouldRespondVolunteerMessage_AndIncrementVolunteers(string input)
    {
        var request = new ProcessWhatsappMessageRequest("user2", input, string.Empty, null, string.Empty);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Contains("Sua disposição em ajudar é muito valiosa", response);
        _infoRepositoryMock.Verify(r => r.IncrementQuantityVolunteersAsync(), Times.Once);
    }

    [Theory]
    [InlineData("3")]
    [InlineData("estou seguro")]
    [InlineData("estou seguro(a)")]
    [InlineData("seguro")]
    [InlineData("informações")]
    [InlineData("info")]
    [InlineData("✅")]
    public async Task Handle_ShouldRespondSafeMessage(string input)
    {
        var request = new ProcessWhatsappMessageRequest("user3", input, string.Empty, null, string.Empty);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Contains("Ficamos felizes em saber que você está em segurança", response);
        _infoRepositoryMock.Verify(r => r.IncrementQuantityHelpAsync(), Times.Never);
        _infoRepositoryMock.Verify(r => r.IncrementQuantityVolunteersAsync(), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData("qualquer coisa")]
    [InlineData(null)]
    public async Task Handle_ShouldRespondWithWelcomeMenu_WhenInputIsUnknown(string input)
    {
        var request = new ProcessWhatsappMessageRequest("user4", input, string.Empty, null, string.Empty);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Contains("Bem-vindo(a) ao Radar Solidário", response);
        _infoRepositoryMock.Verify(r => r.IncrementQuantityHelpAsync(), Times.Never);
        _infoRepositoryMock.Verify(r => r.IncrementQuantityVolunteersAsync(), Times.Never);
    }
}
