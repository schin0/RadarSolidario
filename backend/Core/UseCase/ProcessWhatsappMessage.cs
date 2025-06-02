using Core.Request;
using Infra.Interfaces;
using Infra.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.UseCase;

public class ProcessWhatsappMessage(ILogger<ProcessWhatsappMessage> logger, IInfoRepository infoRepository) : IRequestHandler<ProcessWhatsappMessageRequest, string>
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

    public async Task<string> Handle(ProcessWhatsappMessageRequest request, CancellationToken cancellationToken)
    {
        string userMessageLower = request.MessageBody?.Trim().ToLower() ?? "";
        string userResponseMessage;

        switch (userMessageLower)
        {
            case "1":
            case "preciso de ajuda":
            case "preciso":
            case "🆘":
                userResponseMessage = "Seu pedido de ajuda foi registrado em nosso sistema. Entraremos em contato em breve com mais informações ou para coletar detalhes. Se a situação for de risco extremo e imediato, ligue para os serviços de emergência (190 para Polícia, 193 para Bombeiros, 199 para Defesa Civil).";
                await infoRepository.IncrementQuantityHelpAsync();
                logger.LogInformation("User {Sender} selected: PRECISO DE AJUDA.", request.Sender);
                break;

            case "2":
            case "posso ajudar":
            case "sou voluntario":
            case "sou voluntária":
            case "voluntario":
            case "voluntária":
            case "�":
                userResponseMessage = "Que ótimo! Sua disposição em ajudar é muito valiosa. Para prosseguir com seu cadastro como voluntário(a), por favor, nos informe seu *nome completo*.";
                await infoRepository.IncrementQuantityVolunteersAsync();
                logger.LogInformation("User {Sender} selected: POSSO AJUDAR.", request.Sender);
                break;

            case "3":
            case "estou seguro":
            case "estou seguro(a)":
            case "seguro":
            case "informações":
            case "info":
            case "✅":
                userResponseMessage = "Ficamos felizes em saber que você está em segurança! No momento, para obter informações sobre alertas ou dicas de prevenção, por favor, consulte os canais oficiais da Defesa Civil da sua cidade ou estado. Em breve, teremos mais funcionalidades aqui!";
                logger.LogInformation("User {Sender} selected: ESTOU SEGURO / INFORMAÇÕES.", request.Sender);
                break;

            default:
                userResponseMessage = WelcomeOptionsMenu;
                logger.LogInformation("Sending options menu to {Sender} because the message '{MessageBody}' was not recognized as a direct option.", request.Sender, request.MessageBody);
                break;
        }

        return userResponseMessage;
    }
}
