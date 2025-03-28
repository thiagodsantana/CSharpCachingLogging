using Serilog;

namespace CSharpCachingLogging.Log.API.Services
{
    public class SerilogLoggingService
    {
        public void LogInformation()
        {
            Serilog.Log.Information("Log de Information via Serilog.");
            Serilog.Log.Warning("Log de Warning via Serilog.");
            Serilog.Log.Error("Log de Error via Serilog.");
        }
    }
}