using CSharpCachingLogging.Log.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    //.WriteTo.EventLog("MinimalLoggingDemo", logName: "Application", manageEventSource: true) // "Application" é mais acessível
    .CreateLogger();


builder.Host.UseSerilog();

builder.Services.AddSwaggerGen();

// Configuração do Microsoft.Extensions.Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Logging.AddEventLog();
}

// Adicionando serviços de logging
builder.Services.AddSingleton<DefaultLoggingService>();
builder.Services.AddSingleton<SerilogLoggingService>();

var app = builder.Build();

// Criando rotas que usam os serviços de logging
app.MapGet("/log/microsoft", (DefaultLoggingService logger) =>
{
    logger.LogInformation();
    return Results.Ok("Log registrado com Microsoft.Extensions.Logging");
});

app.MapGet("/log/serilog", (SerilogLoggingService logger) =>
{
    logger.LogInformation();
    return Results.Ok("Log registrado com Serilog");
});

app.Run();
