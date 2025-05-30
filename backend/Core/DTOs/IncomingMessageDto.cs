using System.Text.Json.Serialization;

namespace Core.DTOs;

public class IncomingMessageDto
{
    [JsonPropertyName("webhookType")]
    public string WebhookType { get; set; }

    [JsonPropertyName("sender")]
    public string Sender { get; set; }

    [JsonPropertyName("messageBody")]
    public string MessageBody { get; set; }

    [JsonPropertyName("timestamp")]
    public long? Timestamp { get; set; }

    [JsonPropertyName("messageId")]
    public string MessageId { get; set; }
}
