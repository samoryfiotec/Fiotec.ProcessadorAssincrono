using System.Data;
using Dapper;
using Fiotec.ProcessadorAssincrono.Application.Interfaces;

namespace Fiotec.ProcessadorAssincrono.Infrastructure.Persistence
{
    public class AprovacaoService : IAprovacaoService /* Mudar isso aqui de lugar! */
    {
        private readonly IDbConnection _connection;

        public AprovacaoService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task AprovarAsync(Guid id)
        {
            var sql = "UPDATE Requisicoes SET Aprovada = 1 WHERE Id = @Id AND Aprovada = 0";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
