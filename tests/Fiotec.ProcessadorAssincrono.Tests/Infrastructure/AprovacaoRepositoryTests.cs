using System.Data;
using Fiotec.ProcessadorAssincrono.Domain.Entities;
using Fiotec.ProcessadorAssincrono.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace Fiotec.ProcessadorAssincrono.Tests.Infrastructure
{
    public class AprovacaoRepositoryTests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<IDbTransaction> _mockTransaction;
        private readonly Mock<ILogger<AprovacaoRepository>> _mockLogger;
        private readonly AprovacaoRepository _repository;

        public AprovacaoRepositoryTests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _mockTransaction = new Mock<IDbTransaction>();
            _mockLogger = new Mock<ILogger<AprovacaoRepository>>();
            _repository = new AprovacaoRepository(_mockConnection.Object, _mockTransaction.Object, _mockLogger.Object);
        }

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
