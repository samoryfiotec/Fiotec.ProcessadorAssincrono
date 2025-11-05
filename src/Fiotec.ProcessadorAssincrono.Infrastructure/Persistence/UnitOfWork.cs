using Fiotec.ProcessadorAssincrono.Application.Interfaces;

namespace Fiotec.ProcessadorAssincrono.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task RollbackAsync()
        {
            throw new NotImplementedException();
        }
    }
}
