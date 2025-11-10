namespace Fiotec.ProcessadorAssincrono.Domain.Entities
{
    public class AprovacaoDiariaMessage
    {
        public Guid Id { get; set; }
        public string Pep { get; set; } = string.Empty;
        public string ComentariosAdicionais { get; set; } = string.Empty;
    }
}
