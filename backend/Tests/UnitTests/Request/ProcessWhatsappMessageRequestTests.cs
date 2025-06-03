using Core.Request;

namespace UnitTests.Request;

public class ProcessWhatsappMessageRequestTests
{
    [Fact]
    public void Constructor_SetsAllProperties()
    {
        var sender = "5511999999999";
        var messageBody = "Hello!";
        var webhookType = "message";
        long? timestamp = 1710000000;
        var messageId = "msg-123";

        var request = new ProcessWhatsappMessageRequest(sender, messageBody, webhookType, timestamp, messageId);

        Assert.Equal(sender, request.Sender);
        Assert.Equal(messageBody, request.MessageBody);
        Assert.Equal(webhookType, request.WebhookType);
        Assert.Equal(timestamp, request.Timestamp);
        Assert.Equal(messageId, request.MessageId);
    }

    [Fact]
    public void WithExpression_CreatesModifiedCopy()
    {
        var original = new ProcessWhatsappMessageRequest("A", "B", "C", 1, "D");

        var modified = original with { MessageBody = "NewBody" };

        Assert.Equal("A", modified.Sender);
        Assert.Equal("NewBody", modified.MessageBody);
        Assert.Equal("C", modified.WebhookType);
        Assert.Equal(1, modified.Timestamp);
        Assert.Equal("D", modified.MessageId);
        Assert.NotSame(original, modified);
    }
}
