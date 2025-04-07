using CSharpCachingLogging.Log.API.Services;
using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// =============================
// Configura��o do Serilog
// =============================
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        // Define o n�vel m�nimo de log como Debug para capturar mais detalhes
        .MinimumLevel.Debug()
        // Exibe logs no console, incluindo Debug
        .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
        // Adiciona sa�da de log para o depurador (aparece na janela "Debug Output" do Visual Studio)
        .WriteTo.Debug() 
        .WriteTo.File(
            formatter: new JsonFormatter(), // Salva logs em formato JSON para facilitar a an�lise
            path: "logs/log.txt", // Caminho do arquivo de log
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, // Apenas logs de n�vel Debug ou superiores ser�o gravados
            rollingInterval: RollingInterval.Day) // Cria um novo arquivo de log a cada dia                                                  
        .WriteTo.Seq("http://localhost:5341/", Serilog.Events.LogEventLevel.Debug) // Envia logs para um servidor Seq para an�lise centralizada

        // Enriquecimento de logs com informa��es adicionais
        .Enrich.WithProperty("CSharpCachingLogging", "Empr�stimoConsignado") // Adiciona um campo fixo indicando o nome do sistema
        .Enrich.WithProperty("Vers�o", "1.0.0") // Adiciona a vers�o do sistema nos logs
        .Enrich.WithCorrelationId() // Captura um ID de correla��o para rastreamento de requisi��es distribu�das
        .Enrich.WithEnvironmentName() // Registra o nome do ambiente (ex: "Development", "Production")
        .Enrich.WithEnvironmentUserName(); // Registra o usu�rio do sistema operacional que est� executando a aplica��o
});

// =============================
// Adiciona suporte ao Swagger para documenta��o da API
// =============================
builder.Services.AddSwaggerGen();

// =============================
// Registra os servi�os de logging na inje��o de depend�ncias
// =============================
builder.Services.AddSingleton<DefaultLoggingService>();

var app = builder.Build();

// =============================
// Middleware de logging
// =============================
app.UseSerilogRequestLogging(); // Adiciona logs autom�ticos de requisi��es HTTP (inclui tempo de resposta, status, etc.)

// =============================
// Endpoint de teste para logging
// =============================
app.MapGet("/log", (DefaultLoggingService logger) =>
{
    logger.ExecutarRotina(); // Executa a rotina de logging personalizada dentro do servi�o
    return Results.Ok("Log registrado com Microsoft.Extensions.Logging"); // Retorna um status HTTP 200 (OK)
});

// =============================
// Inicia a aplica��o
// =============================
app.Run();
