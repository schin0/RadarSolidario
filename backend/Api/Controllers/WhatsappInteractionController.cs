using Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Api.Controllers;

[ApiController]
[Route("api/whatsapp")]
public class WhatsAppInteractionController(ILogger<WhatsAppInteractionController> logger, WhatsappService whatsappService) : ControllerBase
{
    private const string WelcomeOptionsMenu =
            "Olá! 👋 Bem-vindo(a) ao Radar Solidário!\n\n" +
            "Como podemos te ajudar hoje ou como você pode nos ajudar?\n\n" +
            "Escolha uma das opções abaixo respondendo com o NÚMERO correspondente:\n\n" +
            "1️⃣ *PRECISO DE AJUDA* 🆘\n" +
            "   _(Se você está em uma situação de risco ou precisa de assistência imediata devido a um evento climático.)_\n\n" +
            "2️⃣ *POSSO AJUDAR / SOU VOLUNTÁRIO(A)* 💪\n" +
            "   _(Se você deseja oferecer sua ajuda, recursos ou se registrar como voluntário.)_\n\n" +
            "3️⃣ *ESTOU SEGURO(A) / OUTRAS INFORMAÇÕES* ✅\n" +
            "   _(Se você está seguro(a), mas gostaria de informações gerais ou reportar algo não urgente.)_";

    [HttpPost("webhook/message-received")]
    public IActionResult ReceiveWebhookMessage([FromBody] IncomingMessageDto messagePayload)
    {
        logger.LogInformation(
            "Webhook received from Node.js: Sender: {Sender}, Message: {MessageBody}, Type: {WebhookType}, MessageId: {MessageId}",
            messagePayload.Sender, messagePayload.MessageBody, messagePayload.WebhookType, messagePayload.MessageId
        );

        string userMessageLower = messagePayload.MessageBody?.Trim().ToLower() ?? "";
        string userResponseMessage;
        
        switch (userMessageLower)
        {
            case "1":
            case "preciso de ajuda":
            case "preciso":
            case "🆘":
                userResponseMessage = "Seu pedido de ajuda foi registrado em nosso sistema. Entraremos em contato em breve com mais informações ou para coletar detalhes. Se a situação for de risco extremo e imediato, ligue para os serviços de emergência (190 para Polícia, 193 para Bombeiros, 199 para Defesa Civil).";
                logger.LogInformation("User {Sender} selected: PRECISO DE AJUDA.", messagePayload.Sender);
                break;

            case "2":
            case "posso ajudar":
            case "sou voluntario":
            case "sou voluntária":
            case "voluntario":
            case "voluntária":
            case "�":
                userResponseMessage = "Que ótimo! Sua disposição em ajudar é muito valiosa. Para prosseguir com seu cadastro como voluntário(a), por favor, nos informe seu *nome completo*.";
                logger.LogInformation("User {Sender} selected: POSSO AJUDAR.", messagePayload.Sender);
                break;

            case "3":
            case "estou seguro":
            case "estou seguro(a)":
            case "seguro":
            case "informações":
            case "info":
            case "✅":
                userResponseMessage = "Ficamos felizes em saber que você está em segurança! No momento, para obter informações sobre alertas ou dicas de prevenção, por favor, consulte os canais oficiais da Defesa Civil da sua cidade ou estado. Em breve, teremos mais funcionalidades aqui!";
                logger.LogInformation("User {Sender} selected: ESTOU SEGURO / INFORMAÇÕES.", messagePayload.Sender);
                break;

            default:
                userResponseMessage = WelcomeOptionsMenu;
                logger.LogInformation("Sending options menu to {Sender} because the message '{MessageBody}' was not recognized as a direct option.", messagePayload.Sender, messagePayload.MessageBody);
                break;
        }

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
    public async Task<IActionResult> TestSendMessage([FromBody] TestMessageDto testPayload)
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

public class TestMessageDto
{
    public string RecipientNumber { get; set; }
    public string MessageContent { get; set; }
}