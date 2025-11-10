namespace Fiotec.ProcessadorAssincrono.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IClasseRepository Classes { get; }
        Task CommitAsync();
        Task RollbackAsync();
    }
}
