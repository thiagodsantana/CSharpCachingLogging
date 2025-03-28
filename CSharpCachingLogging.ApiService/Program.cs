using CSharpCaching.Redis.API.Services;
using StackExchange.Redis;

#region
var connection = "localhost:5000,abortConnect=false,connectTimeout=30000,responseTimeout=30000";
#endregion

// Inicializa o construtor da aplica��o web, permitindo configurar servi�os e middleware
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(); // Certifique-se de que o logging est� registrado
builder.Services.AddSwaggerGen();

// Configura��o do logger usando Serilog
//Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
//builder.Host.UseSerilog();

// Configura a conex�o com o Redis
builder.Services.AddSingleton(ConnectionMultiplexer.Connect(connection));

// Registra outros servi�os, como o EmprestimoService
builder.Services.AddScoped<EmprestimoService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Endpoint para adicionar um empr�stimo
app.MapPost("/emprestimos", async (EmprestimoService servicoEmprestimo, string idEmprestimo, string idUsuario, double valor) =>
{
    await servicoEmprestimo.AdicionarEmprestimoAsync(idEmprestimo, idUsuario, valor);
    return Results.Ok("Empr�stimo adicionado com sucesso");
});

// Endpoint para processar a fila de empr�stimos e envi�-los para a Dataprev
app.MapPost("/emprestimos/processar", async (EmprestimoService servicoEmprestimo) =>
{
    await servicoEmprestimo.ProcessarFilaAsync();
    return Results.Ok("Processamento conclu�do");
});

// Endpoint para iniciar o assinante que ouve as notifica��es
app.MapPost("/emprestimos/iniciar-assinante", (EmprestimoService servicoEmprestimo) =>
{
    // Inicia o assinante para o canal de notifica��es do Redis
    servicoEmprestimo.IniciarAssinante();
    return Results.Ok("Assinante de notifica��es iniciado com sucesso");
});

app.Run();
