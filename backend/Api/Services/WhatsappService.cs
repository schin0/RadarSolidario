using System.Net.Http;
using System.Net.Http.Json; 
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Api.Services;

public class WhatsappService(HttpClient httpClient, IConfiguration configuration, ILogger<WhatsappService> logger)
{
    private readonly string _serviceBaseUrl = configuration.GetValue<string>("WppConnectService:BaseUrl")
            ?? throw new ArgumentNullException("WppConnectService:BaseUrl is not configured in appsettings.json");

    public async Task<bool> SendMessageAsync(string recipientNumber, string messageText)
    {
        var endpoint = $"{_serviceBaseUrl}/send-message";
        var payload = new { recipientNumber, messageText };

        logger.LogInformation("Sending message to {RecipientNumber} via Node.js service: {MessageText}", recipientNumber, messageText);

        try
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(endpoint, payload);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                logger.LogInformation("Message sent successfully via Node.js service. Response: {ResponseBody}", responseBody);
                return true;
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                logger.LogError("Error sending message via Node.js service. Status: {StatusCode}. Error: {ErrorBody}", response.StatusCode, errorBody);
                return false;
            }
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Exception while trying to send message to Node.js service.");
            return false;
        }
    }
}
