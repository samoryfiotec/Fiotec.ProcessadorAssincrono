using Fiotec.ProcessadorAssincrono.Application.Validators;
using Fiotec.ProcessadorAssincrono.Domain.DTOs;
using Shouldly;

namespace Fiotec.ProcessadorAssincrono.Tests.Application;

public class LoteAprovacaoRequestValidatorTests
{
    private readonly LoteAprovacaoRequestValidator _validator;

    public LoteAprovacaoRequestValidatorTests()
    {
        _validator = new LoteAprovacaoRequestValidator();
    }

    [Fact]
    public void Deve_RetornarErro_QuandoSolicitacoesForVazia()
    {
        // Arrange
        var request = new LoteAprovacaoRequest(new List<Guid>());

        // Act
        var resultado = _validator.Validate(request);

        // Assert
        resultado.IsValid.ShouldBeFalse();
        resultado.Errors.Any(e => e.ErrorMessage.Contains("pelo menos uma solicitação")).ShouldBeTrue();
    }

    [Fact]
    public void Deve_RetornarErro_QuandoSolicitacoesContemGuidEmpty()
    {
        // Arrange
        var request = new LoteAprovacaoRequest(new List<Guid> { Guid.Empty, Guid.NewGuid() });

        // Act
        var resultado = _validator.Validate(request);

        // Assert
        resultado.IsValid.ShouldBeFalse();
        resultado.Errors.Any(e => e.ErrorMessage.Contains("GUIDs devem ser válidos")).ShouldBeTrue();
    }

    [Fact]
    public void Deve_RetornarErro_QuandoSolicitacoesContemDuplicados()
    {
        // Arrange
        var guidDuplicado = Guid.NewGuid();
        var request = new LoteAprovacaoRequest(new List<Guid> { guidDuplicado, guidDuplicado });

        // Act
        var resultado = _validator.Validate(request);

        // Assert
        resultado.IsValid.ShouldBeFalse();
        resultado.Errors.Any(e => e.ErrorMessage.Contains("não pode conter GUIDs duplicados")).ShouldBeTrue();
    }

    [Fact]
    public void Deve_PassarValidacao_QuandoSolicitacoesValidas()
    {
        // Arrange
        var request = new LoteAprovacaoRequest(new List<Guid> { Guid.NewGuid(), Guid.NewGuid() });

        // Act
        var resultado = _validator.Validate(request);

        // Assert
        resultado.IsValid.ShouldBeTrue();
    }
}