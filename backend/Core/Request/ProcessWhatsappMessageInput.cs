using MediatR;

namespace Core.Request;

public record ProcessWhatsappMessageRequest(
    string Sender, 
    string MessageBody, 
    string WebhookType, 
    long? Timestamp, 
    string MessageId
): IRequest<string>;
