namespace Fiotec.ProcessadorAssincrono.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IClasseRepository Classes { get; }
        Task CommitAsync();
        Task RollbackAsync();
    }
}
