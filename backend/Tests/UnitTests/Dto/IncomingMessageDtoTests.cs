using System.Text.Json;
using Core.DTOs;

namespace UnitTests.Dto;

public class IncomingMessageDtoTests
{
    [Fact]
    public void Should_Serialize_And_Deserialize_Correctly()
    {
        var dto = new IncomingMessageDto
        {
            WebhookType = "message",
            Sender = "user1",
            MessageBody = "Hello",
            Timestamp = 1234567890,
            MessageId = "msg-001"
        };

        var json = JsonSerializer.Serialize(dto);
        var deserialized = JsonSerializer.Deserialize<IncomingMessageDto>(json)!;

        Assert.Equal(dto.WebhookType, deserialized.WebhookType);
        Assert.Equal(dto.Sender, deserialized.Sender);
        Assert.Equal(dto.MessageBody, deserialized.MessageBody);
        Assert.Equal(dto.Timestamp, deserialized.Timestamp);
        Assert.Equal(dto.MessageId, deserialized.MessageId);
    }

    [Fact]
    public void Should_Deserialize_From_Json_With_Expected_Properties()
    {
        var json = @"{
            ""webhookType"": ""event"",
            ""sender"": ""user2"",
            ""messageBody"": ""Test"",
            ""timestamp"": 987654321,
            ""messageId"": ""msg-002""
        }";

        var dto = JsonSerializer.Deserialize<IncomingMessageDto>(json)!;

        Assert.Equal("event", dto.WebhookType);
        Assert.Equal("user2", dto.Sender);
        Assert.Equal("Test", dto.MessageBody);
        Assert.Equal(987654321, dto.Timestamp);
        Assert.Equal("msg-002", dto.MessageId);
    }
}
