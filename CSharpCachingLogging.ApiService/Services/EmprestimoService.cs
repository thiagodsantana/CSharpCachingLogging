using StackExchange.Redis;

namespace CSharpCaching.Redis.API.Services
{
    public class EmprestimoService(ConnectionMultiplexer redis, ILogger<EmprestimoService> logger)
    {
        // Serviço responsável pelo gerenciamento dos empréstimos

        private readonly IDatabase _redis = redis.GetDatabase();
        private readonly ISubscriber _subscriber = redis.GetSubscriber();
        private const string FilaEmprestimos = "filaEmprestimos";
        private const string DadosEmprestimos = "dadosEmprestimos";
        private const string UsuariosEmprestimos = "usuariosEmprestimos";
        private const string PrioridadeEmprestimos = "prioridadeEmprestimos";
        private const string CanalEmprestimos = "notificacoesEmprestimos";

        // Método para adicionar um novo empréstimo diretamente no Redis
        public async Task AdicionarEmprestimoAsync(string idEmprestimo, string idUsuario, double valor)
        {
            logger.LogInformation("Registrando empréstimo {IdEmprestimo} no Redis", idEmprestimo);

            // Armazenando String
            await _redis.StringSetAsync("tokenAPI", "938333-84474-85858-216300", TimeSpan.FromMinutes(30));
            var valorArmazenado = await _redis.StringGetAsync("tokenAPI");


            var dadosEmprestimo = new HashEntry[]
            {
                new("IdUsuario", idUsuario),
                new("Valor", valor),
                new("Timestamp", DateTime.UtcNow.ToString())
            };
            // Armazena os detalhes do empréstimo no Redis como um hash
            await _redis.HashSetAsync($"{DadosEmprestimos}:{idEmprestimo}", dadosEmprestimo);

            // Adiciona o ID do empréstimo na fila para processamento futuro (fim da fila)
            await _redis.ListRightPushAsync(FilaEmprestimos, idEmprestimo);

            // Mantém um conjunto de usuários que solicitaram empréstimos
            // Não permitem valores duplicados e não garantem ordem.
            await _redis.SetAddAsync(UsuariosEmprestimos, idUsuario);

            // Adiciona o empréstimo a um conjunto ordenado baseado no valor do empréstimo
            await _redis.SortedSetAddAsync(PrioridadeEmprestimos, idEmprestimo, valor);

            // Publica uma mensagem notificando sobre o novo empréstimo
            await _redis.PublishAsync(CanalEmprestimos, $"Novo empréstimo criado: {idEmprestimo}");

            logger.LogInformation("Empréstimo {IdEmprestimo} adicionado com sucesso", idEmprestimo);
        }

        // Método que processa a fila de empréstimos e os envia para a Dataprev
        public async Task ProcessarFilaAsync()
        {
            logger.LogInformation("Iniciando processamento da fila de empréstimos.");
            while (true)
            {
                // Remove e obtém o próximo ID de empréstimo na fila
                var idEmprestimo = await _redis.ListLeftPopAsync(FilaEmprestimos);
                if (idEmprestimo.IsNullOrEmpty) break;

                logger.LogInformation("Processando envio do empréstimo {IdEmprestimo} para a Dataprev", idEmprestimo);
                await Task.Delay(1000); // Simulando envio
                logger.LogInformation("Empréstimo {IdEmprestimo} enviado com sucesso para a Dataprev", idEmprestimo);
            }
            logger.LogInformation("Processamento da fila concluído.");
        }

        // Método para iniciar o assinante de notificações de novos empréstimos
        public void IniciarAssinante()
        {
            // Inscreve-se no canal de notificações de empréstimos
            _subscriber.Subscribe(CanalEmprestimos, (channel, message) =>
            {
                // Quando uma mensagem for publicada, o sistema vai logar ou processar a notificação
                logger.LogInformation("Recebido no canal {CanalEmprestimos}: {Message}", channel, message);
                // Aqui você pode adicionar lógicas extras de processamento ou notificação para os usuários
            });

            logger.LogInformation("Assinante de notificações de empréstimos iniciado.");
        }
    }
}
