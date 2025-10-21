namespace Fiotec.ProcessadorAssincrono.Application.Interfaces
{
    public interface IProcessadorQueue
    {
        Task EnfileirarAsync(Guid id);
    }

}
