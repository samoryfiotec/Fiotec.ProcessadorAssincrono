using System.Threading.Channels;
using Fiotec.ProcessadorAssincrono.Application.Interfaces;
using Fiotec.ProcessadorAssincrono.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace Fiotec.ProcessadorAssincrono.Infrastructure.BackgroundServices
{
    public class ProcessadorQueueService : BackgroundService, IProcessadorQueue
    {
        private readonly Channel<Aprovacao> _channel;
        private readonly IServiceProvider _serviceProvider;

        public ProcessadorQueueService(Channel<Aprovacao> channel, IServiceProvider serviceProvider)
        {
            _channel = channel;
            _serviceProvider = serviceProvider;
        }

        public async Task EnfileirarAsync(Guid id, string pep, string comentariosAdicionais, DateTime dataAprovacao)
        {
            var mensagem = new Aprovacao { Id = id, Pep = pep, ComentariosAdicionais = comentariosAdicionais, DataAprovacao = dataAprovacao };
            await _channel.Writer.WriteAsync(mensagem);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(attempt),
                    (ex, ts) => Console.WriteLine($"Erro: {ex.Message}. Tentando novamente..."));

            while (await _channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var mensagem = await _channel.Reader.ReadAsync(stoppingToken);

                await retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var service = scope.ServiceProvider.GetRequiredService<IAprovacaoService>();

                        await service.AprovarAsync(mensagem.Id, mensagem.Pep, mensagem.ComentariosAdicionais, mensagem.DataAprovacao);
                        Console.WriteLine($"Processado: {mensagem.Id}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Falha ao processar {mensagem.Id}: {ex.Message}");
                        throw;
                    }
                });
            }
        }
    }
}