public static partial class LoggingSourceGenerator
{
    [LoggerMessage(1, LogLevel.Information, "Requisição recebida. RequestId: {requestId}, UserId: {userId}")]
    public static partial void LogRequestReceived(this ILogger logger, string requestId, string userId);

    [LoggerMessage(2, LogLevel.Debug, "Debug Info: {info}, RequestId: {requestId}, UserId: {userId}")]
    public static partial void LogDebugInfo(this ILogger logger, string info, string requestId, string userId);

    [LoggerMessage(3, LogLevel.Warning, "Requisição recebida. RequestId: {requestId}, UserId: {userId}")]
    public static partial void LogWarning(this ILogger logger, string requestId, string userId);

    [LoggerMessage(4, LogLevel.Error, "Erro: {error}, RequestId: {requestId}, UserId: {userId}")]
    public static partial void LogErrorOccurred(this ILogger logger, string error, string requestId, string userId);
}