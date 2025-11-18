using Fiotec.ProcessadorAssincrono.Application.Interfaces;
using Fiotec.ProcessadorAssincrono.Domain.Entities;
using Fiotec.ProcessadorAssincrono.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Fiotec.ProcessadorAssincrono.Infrastructure.Services
{
    public class AprovacaoService : IAprovacaoService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AprovacaoService> _logger;

        public AprovacaoService(IUnitOfWork uow, ILogger<AprovacaoService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task AprovarAsync(Guid id, string pep, string comentariosAdicionais, DateTime dataAprovacao)
        {
            try
            {
                var aprovacao = new Aprovacao { Id = id, Pep = pep, ComentariosAdicionais = comentariosAdicionais, DataAprovacao = dataAprovacao };

                await _uow.Aprovacoes.InserirAsync(aprovacao);
                await _uow.CommitAsync();

                _logger.LogInformation("Aprovação {Id} salva com sucesso.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao aprovar {Id}, rollback iniciado.", id);
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task InserirAsync(Aprovacao aprovacao)
        {
            if (aprovacao == null)
            {
                _logger.LogWarning("Tentativa de inserir uma aprovação nula.");
                throw new ArgumentNullException(nameof(aprovacao), "Aprovação não pode ser nula.");
            }

            try
            {
                await _uow.Aprovacoes.InserirAsync(aprovacao);
                await _uow.CommitAsync();

                _logger.LogInformation("Aprovação {Id} inserida com sucesso.", aprovacao.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir aprovação {Id}, rollback iniciado.", aprovacao.Id);
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}