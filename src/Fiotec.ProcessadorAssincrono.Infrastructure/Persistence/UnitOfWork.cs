using System.Data;
using Fiotec.ProcessadorAssincrono.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Fiotec.ProcessadorAssincrono.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private IDbConnectionFactory connectionFactory;
        private readonly ILogger _logger;      

        //public IClasseRepository Classes { get; }

        public UnitOfWork(IDbConnectionFactory connectionFactory, ILogger logger)
        {
            this.connectionFactory = connectionFactory;
            _logger = logger;
            _connection = connectionFactory.CreateConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public UnitOfWork(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task CommitAsync()
        {
            try
            {
                _transaction?.Commit();
                _logger.LogInformation("Transação confirmada com sucesso.");
            }
            catch
            {
                _transaction?.Rollback();
                _logger.LogError("Transação revertida.");
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
                _logger.LogInformation("Conexão com o banco de dados fechada.");
            }

            await Task.CompletedTask;
        }

        public async Task RollbackAsync()
        {
            try
            {
                _transaction?.Rollback();
                _logger.LogInformation("Transação revertida com sucesso.");
            }
            finally
            {
                _transaction?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
                _logger.LogInformation("Conexão com o banco de dados fechada depois de transação revertida.");
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
