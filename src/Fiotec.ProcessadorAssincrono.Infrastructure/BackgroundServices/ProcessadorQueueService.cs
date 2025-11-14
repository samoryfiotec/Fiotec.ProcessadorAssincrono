using System.Threading.Channels;
using Fiotec.ProcessadorAssincrono.Application.Interfaces;
using Fiotec.ProcessadorAssincrono.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        public async Task EnfileirarAsync(Guid id, string pep, string comentariosAdicionais)
        {
            var mensagem = new Aprovacao { Id = id, Pep = pep, ComentariosAdicionais = comentariosAdicionais };
            await _channel.Writer.WriteAsync(mensagem);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var mensagem = await _channel.Reader.ReadAsync(stoppingToken);

                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IAprovacaoService>();

                await service.AprovarAsync(mensagem.Id, mensagem.Pep, mensagem.ComentariosAdicionais);
            }
        }
    }
}