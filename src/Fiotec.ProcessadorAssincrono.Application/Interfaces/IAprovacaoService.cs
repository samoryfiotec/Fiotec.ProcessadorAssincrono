using Fiotec.ProcessadorAssincrono.Domain.Entities;

namespace Fiotec.ProcessadorAssincrono.Application.Interfaces
{
    public interface IAprovacaoService
    {
        Task AprovarAsync(Guid id, string pep, string comentariosAdicionais, DateTime dataAprovacao);

        Task InserirAsync(Aprovacao aprovacao);
    }


}
