using CSharpCaching.Redis.API.Services;
using StackExchange.Redis;

#region
var connection = "localhost:5000,abortConnect=false,connectTimeout=30000,responseTimeout=30000";
#endregion

// Inicializa o construtor da aplicação web, permitindo configurar serviços e middleware
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(); // Certifique-se de que o logging está registrado
builder.Services.AddSwaggerGen();

// Configuração do logger usando Serilog
//Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
//builder.Host.UseSerilog();

// Configura a conexão com o Redis
builder.Services.AddSingleton(ConnectionMultiplexer.Connect(connection));

// Registra outros serviços, como o EmprestimoService
builder.Services.AddScoped<EmprestimoService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Endpoint para adicionar um empréstimo
app.MapPost("/emprestimos", async (EmprestimoService servicoEmprestimo, string idEmprestimo, string idUsuario, double valor) =>
{
    await servicoEmprestimo.AdicionarEmprestimoAsync(idEmprestimo, idUsuario, valor);
    return Results.Ok("Empréstimo adicionado com sucesso");
});

// Endpoint para processar a fila de empréstimos e enviá-los para a Dataprev
app.MapPost("/emprestimos/processar", async (EmprestimoService servicoEmprestimo) =>
{
    await servicoEmprestimo.ProcessarFilaAsync();
    return Results.Ok("Processamento concluído");
});

// Endpoint para iniciar o assinante que ouve as notificações
app.MapPost("/emprestimos/iniciar-assinante", (EmprestimoService servicoEmprestimo) =>
{
    // Inicia o assinante para o canal de notificações do Redis
    servicoEmprestimo.IniciarAssinante();
    return Results.Ok("Assinante de notificações iniciado com sucesso");
});

app.Run();
