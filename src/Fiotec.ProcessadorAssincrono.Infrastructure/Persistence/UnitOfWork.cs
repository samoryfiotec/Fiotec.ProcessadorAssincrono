using System.Data;
using Fiotec.ProcessadorAssincrono.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Fiotec.ProcessadorAssincrono.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private readonly ILogger<UnitOfWork> _logger;

        public IAprovacaoRepository Aprovacoes { get; }

        public UnitOfWork(IDbConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UnitOfWork>();

            _connection = connectionFactory.CreateConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();

            Aprovacoes = new AprovacaoRepository(_connection, _transaction, loggerFactory.CreateLogger<AprovacaoRepository>());
        }

        public async Task CommitAsync()
        {
            try
            {
                _transaction?.Commit();
                _logger.LogInformation("Transaction committed successfully.");
            }
            catch (Exception ex)
            {
                _transaction?.Rollback();
                _logger.LogError(ex, "Transaction rolled back due to error during commit.");
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
                _logger.LogInformation("Database connection closed after commit.");
            }

            await Task.CompletedTask;
        }

        public async Task RollbackAsync()
        {
            try
            {
                _transaction?.Rollback();
                _logger.LogInformation("Transaction rolled back.");
            }
            finally
            {
                _transaction?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
                _logger.LogInformation("Database connection closed after rollback.");
            }

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}