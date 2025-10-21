namespace Fiotec.ProcessadorAssincrono.Domain.Entities
{
    public class Requisicao
    {
        public Guid Id { get; set; }
        public bool Aprovada { get; set; }
        public DateTime DataSolicitacao { get; set; }
    }

}
