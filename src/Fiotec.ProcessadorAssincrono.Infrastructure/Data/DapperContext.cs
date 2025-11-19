using System.Data;
using Fiotec.ProcessadorAssincrono.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Fiotec.ProcessadorAssincrono.Infrastructure.Data
{
    public class DapperContext : IDbConnectionFactory
    {

        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IDbConnection CreateConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new InvalidOperationException("Connection string não pode ser nula ou vazia");

            return new SqlConnection(_connectionString);
        }

    }
}
