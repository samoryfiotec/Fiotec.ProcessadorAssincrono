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
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);

    }
}
