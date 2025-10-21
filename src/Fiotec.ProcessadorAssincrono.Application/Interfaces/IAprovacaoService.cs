namespace Fiotec.ProcessadorAssincrono.Application.Interfaces
{
    public interface IAprovacaoService
    {
        Task AprovarAsync(Guid id);
    }

}
