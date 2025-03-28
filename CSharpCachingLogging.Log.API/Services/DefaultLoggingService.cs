namespace CSharpCachingLogging.Log.API.Services
{
    public class DefaultLoggingService(ILogger<DefaultLoggingService> logger)
    {
        public void LogInformation()
        {
            logger.LogRequestReceived();
            logger.LogDebugInfo("Debug via Source Generator.");
            logger.LogErrorOccurred("Erro via Source Generator.");
        }
    }
}