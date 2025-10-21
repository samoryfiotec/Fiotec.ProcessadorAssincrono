using System.Threading.Channels;
using Fiotec.ProcessadorAssincrono.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fiotec.ProcessadorAssincrono.Infrastructure.BackgroundServices
{
    public class ProcessadorQueueService : BackgroundService, IProcessadorQueue
    {
        private readonly Channel<Guid> _channel = Channel.CreateUnbounded<Guid>();
        private readonly IServiceProvider _serviceProvider;

        public ProcessadorQueueService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task EnfileirarAsync(Guid id)
        {
            await _channel.Writer.WriteAsync(id);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var id = await _channel.Reader.ReadAsync(stoppingToken);

                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IAprovacaoService>();

                await service.AprovarAsync(id);
            }
        }
    }
}
