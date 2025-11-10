using System.Data;

namespace Fiotec.ProcessadorAssincrono.Infrastructure.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
