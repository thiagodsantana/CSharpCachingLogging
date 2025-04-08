public static partial class LoggingSourceGenerator
{
    [LoggerMessage(1, LogLevel.Information, "Requisição recebida. Date: {date}, RequestId: {requestId}, UserId: {userId}")]
    public static partial void LogRequestReceived(this ILogger logger, string requestId, string userId, string date);

    [LoggerMessage(2, LogLevel.Debug, "Debug Info: {info}, Date: {date}, RequestId: {requestId}, UserId: {userId}")]
    public static partial void LogDebugInfo(this ILogger logger, string info, string requestId, string userId, string date);

    [LoggerMessage(4, LogLevel.Error, "Erro: {error}, Date: {date}, RequestId: {requestId}, UserId: {userId}")]
    public static partial void LogErrorOccurred(this ILogger logger, string error, string requestId, string userId, string date);



    // Métodos de extensão para preencher a data automaticamente
    public static void LogRequestReceivedAuto(this ILogger logger, string requestId, string userId) =>
        logger.LogRequestReceived(requestId, userId, DateTime.UtcNow.ToString("o"));

    public static void LogDebugInfoAuto(this ILogger logger, string info, string requestId, string userId) =>
        logger.LogDebugInfo(info, requestId, userId, DateTime.UtcNow.ToString("o"));

    public static void LogErrorOccurredAuto(this ILogger logger, string error, string requestId, string userId) =>
        logger.LogErrorOccurred(error, requestId, userId, DateTime.UtcNow.ToString("o"));
}
