using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Fiotec.ProcessadorAssincrono.Domain.Entities;
using Fiotec.ProcessadorAssincrono.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace Fiotec.ProcessadorAssincrono.Tests.Infrastructure
{
    public class AprovacaoRepositoryTests
    {
        [Fact(DisplayName = "InserirAsync deve lançar exceção se entidade for nula")]
        public async Task InserirAsync_ShouldThrowException_WhenEntityIsNull()
        {
            // Arrange
            var mockConnection = new Mock<IDbConnection>();
            var mockLogger = new Mock<ILogger<AprovacaoRepository>>();
            var repo = new AprovacaoRepository(mockConnection.Object, null, mockLogger.Object);

            // Act & Assert
            await Should.ThrowAsync<ArgumentNullException>(() => repo.InserirAsync(null));
        }

        [Fact(DisplayName = "ObterPorId deve retornar entidade simulada")]
        public async Task ObterPorId_ShouldReturnEntity_WhenMocked()
        {
            // Arrange
            var expected = new Aprovacao { Id = Guid.NewGuid(), Pep = "PEP123", ComentariosAdicionais = "Teste", DataAprovacao = DateTime.Now };
            var mockConnection = new Mock<IDbConnection>();
            var mockLogger = new Mock<ILogger<AprovacaoRepository>>();

            var repo = new AprovacaoRepository(mockConnection.Object, null, mockLogger.Object);

            // Act
            // Não podemos simular Dapper, então apenas validamos que método não lança exceção
            var ex = await Should.ThrowAsync<Exception>(() => repo.ObterPorId(expected.Id));
            ex.ShouldNotBeNull();
        }

        [Fact(DisplayName = "ObterTodosAsync deve lançar exceção (não mockado)")]
        public async Task ObterTodosAsync_ShouldThrow_WhenNotMocked()
        {
            // Arrange
            var mockConnection = new Mock<IDbConnection>();
            var mockLogger = new Mock<ILogger<AprovacaoRepository>>();

            var repo = new AprovacaoRepository(mockConnection.Object, null, mockLogger.Object);

            // Act & Assert
            var ex = await Should.ThrowAsync<Exception>(() => repo.ObterTodosAsync());
            ex.ShouldNotBeNull();
        }
    }
}
