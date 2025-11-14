namespace Fiotec.ProcessadorAssincrono.Application.Interfaces
{
    public interface IAprovacaoService
    {
        Task AprovarAsync(Guid id, string pep, string comentariosAdicionais);
    }


}
