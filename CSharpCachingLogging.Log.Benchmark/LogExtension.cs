using Microsoft.Extensions.Logging;

namespace CSharpCachingLogging.Log.Benchmark
{
    public static partial class LogExtensions
    {
        [LoggerMessage(1, LogLevel.Information, "Requisição recebida. RequestId: {requestId}, UserId: {userId}")]
        public static partial void LogRequestReceived(this ILogger logger, string requestId, string userId);
    }
}
