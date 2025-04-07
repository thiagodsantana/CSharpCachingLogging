using CSharpCachingLogging.Log.API.Services;
using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// =============================
// Configuração do Serilog
// =============================
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        // Define o nível mínimo de log como Debug para capturar mais detalhes
        .MinimumLevel.Debug()
        // Exibe logs no console, incluindo Debug
        .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
        // Adiciona saída de log para o depurador (aparece na janela "Debug Output" do Visual Studio)
        .WriteTo.Debug() 
        .WriteTo.File(
            formatter: new JsonFormatter(), // Salva logs em formato JSON para facilitar a análise
            path: "logs/log.txt", // Caminho do arquivo de log
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, // Apenas logs de nível Debug ou superiores serão gravados
            rollingInterval: RollingInterval.Day) // Cria um novo arquivo de log a cada dia                                                  
        .WriteTo.Seq("http://localhost:5341/", Serilog.Events.LogEventLevel.Debug) // Envia logs para um servidor Seq para análise centralizada

        // Enriquecimento de logs com informações adicionais
        .Enrich.WithProperty("CSharpCachingLogging", "EmpréstimoConsignado") // Adiciona um campo fixo indicando o nome do sistema
        .Enrich.WithProperty("Versão", "1.0.0") // Adiciona a versão do sistema nos logs
        .Enrich.WithCorrelationId() // Captura um ID de correlação para rastreamento de requisições distribuídas
        .Enrich.WithEnvironmentName() // Registra o nome do ambiente (ex: "Development", "Production")
        .Enrich.WithEnvironmentUserName(); // Registra o usuário do sistema operacional que está executando a aplicação
});

// =============================
// Adiciona suporte ao Swagger para documentação da API
// =============================
builder.Services.AddSwaggerGen();

// =============================
// Registra os serviços de logging na injeção de dependências
// =============================
builder.Services.AddSingleton<DefaultLoggingService>();

var app = builder.Build();

// =============================
// Middleware de logging
// =============================
app.UseSerilogRequestLogging(); // Adiciona logs automáticos de requisições HTTP (inclui tempo de resposta, status, etc.)

// =============================
// Endpoint de teste para logging
// =============================
app.MapGet("/log", (DefaultLoggingService logger) =>
{
    logger.ExecutarRotina(); // Executa a rotina de logging personalizada dentro do serviço
    return Results.Ok("Log registrado com Microsoft.Extensions.Logging"); // Retorna um status HTTP 200 (OK)
});

// =============================
// Inicia a aplicação
// =============================
app.Run();
