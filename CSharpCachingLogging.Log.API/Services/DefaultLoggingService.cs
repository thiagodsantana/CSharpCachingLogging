using Microsoft.Extensions.Logging;
using System;

namespace CSharpCachingLogging.Log.API.Services
{
    public class DefaultLoggingService(ILogger<DefaultLoggingService> logger)
    {
        public void ExecutarRotina()
        {
            string requestId = Guid.NewGuid().ToString(); // Gera um RequestId se não for fornecido
            string userId = $"user-{new Random().Next(1000, 9999)}"; // Simula um UserId aleatório

            // Source Generation
            logger.LogRequestReceivedAuto(requestId, userId);
            logger.LogDebugInfoAuto("Executando rotina", requestId, userId);
            logger.LogErrorOccurredAuto("Erro ao executar rotina.", requestId, userId);

            // Ilogger
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Ação registrada: {Action} - {Details} | RequestId: {RequestId} | UserId: {UserId}",
                                       nameof(DefaultLoggingService), "Gerando log de information.", requestId, userId);
            }

            logger.LogDebug("Debugging detalhes: {Details} | RequestId: {RequestId} | UserId: {UserId}",
                             "Gerando log de debug.", requestId, userId);

            logger.LogWarning("Ação registrada: {Action} - {Details} | RequestId: {RequestId} | UserId: {UserId}",
                                   nameof(DefaultLoggingService), "Gerando log de Warning.", requestId, userId);

            logger.LogError("Erro detectado: {Details} | RequestId: {RequestId} | UserId: {UserId}",
                             "Gerando log de error.", requestId, userId);
        }
    }
}