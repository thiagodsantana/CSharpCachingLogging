using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CSharpCachingLogging.Log.Benchmark
{
    [Config(typeof(CustomBenchmarkConfig))]
    [MemoryDiagnoser]
    public class ApiBenchmark
    {
        private readonly ILogger<ApiBenchmark> _logger;

        public ApiBenchmark()
        {
            // Usa um Logger que não gera saída (para evitar poluição no console)
            _logger = NullLogger<ApiBenchmark>.Instance;
        }

        [Benchmark(Baseline = true)]
        public string SemSourceGeneration()
        {
            _logger.LogInformation(message: "UserId: {UserId} | RequestId: {RequestId}", "User-001", "001");
            return "User-001";
        }

        [Benchmark]
        public string ComSourceGeneration()
        {
            _logger.LogRequestReceived("User-001", "001");
            return "User-001";
        }
    }



    // Configuração personalizada do benchmark
    public class CustomBenchmarkConfig : ManualConfig
    {
        public CustomBenchmarkConfig()
        {
            AddExporter(MarkdownExporter.GitHub); // Gera relatório em Markdown
            AddExporter(CsvExporter.Default); // Gera relatório em CSV
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ApiBenchmark>();
        }
    }
}