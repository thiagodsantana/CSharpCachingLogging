using Microsoft.Extensions.Logging;

public static partial class LoggingSourceGenerator
{
    [LoggerMessage(1, LogLevel.Information, "Requisição recebida.")]
    public static partial void LogRequestReceived(this ILogger logger);

    [LoggerMessage(2, LogLevel.Debug, "Debug Info: {info}")]
    public static partial void LogDebugInfo(this ILogger logger, string info);

    [LoggerMessage(3, LogLevel.Error, "Erro: {error}")]
    public static partial void LogErrorOccurred(this ILogger logger, string error);
}
