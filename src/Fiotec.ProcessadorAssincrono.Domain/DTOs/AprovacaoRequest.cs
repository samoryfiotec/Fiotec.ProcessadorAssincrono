namespace Fiotec.ProcessadorAssincrono.Domain.DTOs
{
    public record AprovacaoRequest(string Pep, string ComentariosAdicionais, DateTime DataAprovacao);
}
