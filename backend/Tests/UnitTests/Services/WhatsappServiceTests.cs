using Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;

namespace UnitTests.Services;

public class WhatsappServiceTests
{
    private readonly Mock<ILogger<WhatsappService>> _loggerMock = new();
    private readonly IConfiguration _configuration;

    public WhatsappServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "WppConnectService:BaseUrl", "http://localhost:3000" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
    }

    private static HttpClient CreateHttpClient(HttpResponseMessage responseMessage)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();

        return new HttpClient(handlerMock.Object);
    }

    [Fact]
    public async Task SendMessageAsync_ReturnsTrue_OnSuccess()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"result\":\"success\"}")
        };
        var httpClient = CreateHttpClient(response);
        var service = new WhatsappService(httpClient, _configuration, _loggerMock.Object);

        var result = await service.SendMessageAsync("5511999999999", "Hello!");

        Assert.True(result);
    }

    [Fact]
    public async Task SendMessageAsync_ReturnsFalse_OnFailureStatus()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("{\"error\":\"invalid number\"}")
        };
        var httpClient = CreateHttpClient(response);
        var service = new WhatsappService(httpClient, _configuration, _loggerMock.Object);

        var result = await service.SendMessageAsync("invalid", "Hello!");

        Assert.False(result);
    }

    [Fact]
    public async Task SendMessageAsync_ReturnsFalse_OnHttpRequestException()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var httpClient = new HttpClient(handlerMock.Object);
        var service = new WhatsappService(httpClient, _configuration, _loggerMock.Object);

        var result = await service.SendMessageAsync("5511999999999", "Hello!");

        Assert.False(result);
    }

    [Fact]
    public void Constructor_ThrowsException_WhenBaseUrlMissing()
    {
        var config = new ConfigurationBuilder().Build();
        var httpClient = new HttpClient();

        var ex = Assert.Throws<ArgumentNullException>(() =>
            new WhatsappService(httpClient, config, _loggerMock.Object));
        Assert.Contains("WppConnectService:BaseUrl", ex.Message);
    }
}
