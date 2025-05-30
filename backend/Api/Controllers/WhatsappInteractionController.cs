using Api.Services;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Core.DTOs;
using Core.Request;

namespace Api.Controllers;

[ApiController]
[Route("api/whatsapp")]
public class WhatsAppInteractionController(
    ILogger<WhatsAppInteractionController> logger,
    WhatsappService whatsappService,
    IMediator mediator) : ControllerBase
{
    [HttpPost("webhook/message-received")]
    public async Task<IActionResult> ReceiveWebhookMessage([FromBody] IncomingMessageDto messagePayload)
    {
        logger.LogInformation(
            "Webhook received from Node.js: Sender: {Sender}, Message: {MessageBody}, Type: {WebhookType}, MessageId: {MessageId}",
            messagePayload.Sender, messagePayload.MessageBody, messagePayload.WebhookType, messagePayload.MessageId
        );

        var command = new ProcessWhatsappMessageRequest
        (
            messagePayload.Sender,
            messagePayload.MessageBody,
            messagePayload.WebhookType,
            messagePayload.Timestamp,
            messagePayload.MessageId
        );

        var userResponseMessage = await mediator.Send(command);

        if (!string.IsNullOrEmpty(userResponseMessage))
        {
            _ = Task.Run(() => whatsappService.SendMessageAsync(messagePayload.Sender, userResponseMessage));
        }
        else
        {
            logger.LogWarning("No response message was defined for user {Sender} with message '{MessageBody}'.", messagePayload.Sender, messagePayload.MessageBody);
        }

        return Ok(new { status = "Webhook received and processing initiated." });
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> TestSendMessage([FromBody] MessageDto testPayload)
    {
        if (string.IsNullOrEmpty(testPayload.RecipientNumber) || string.IsNullOrEmpty(testPayload.MessageContent))
        {
            return BadRequest("RecipientNumber and MessageContent are required.");
        }
        logger.LogInformation("Test send message to: {RecipientNumber}, Message: {MessageContent}", testPayload.RecipientNumber, testPayload.MessageContent);

        bool success = await whatsappService.SendMessageAsync(testPayload.RecipientNumber, testPayload.MessageContent);
        if (success)
        {
            return Ok(new { status = "Send attempt initiated." });
        }
        return StatusCode(500, new { status = "Failed to initiate send attempt." });
    }
}