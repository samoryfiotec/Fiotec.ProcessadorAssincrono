using FluentValidation;
using Fiotec.ProcessadorAssincrono.Domain.DTOs;

namespace Fiotec.ProcessadorAssincrono.Application.Validators
{
    public class AprovacaoRequestValidator : AbstractValidator<AprovacaoRequest>
    {
        public AprovacaoRequestValidator()
        {
            RuleFor(x => x.Pep).NotEmpty().WithMessage("O campo PEP é obrigatório.");
            RuleFor(x => x.ComentariosAdicionais).MaximumLength(500)
                .WithMessage("Comentários adicionais não podem exceder 500 caracteres.");
        }
    }
}
