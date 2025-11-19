using System;
using System.Data;
using Fiotec.ProcessadorAssincrono.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Fiotec.ProcessadorAssincrono.Tests.Infrastructure
{
    public class DapperContextTests
    {
        [Fact(DisplayName = "Construtor deve inicializar corretamente com a connection string")]
        public void Constructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var expectedConnectionString = "Server=localhost;Database=TestDb;User Id=sa;Password=123;";
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ConnectionStrings:DefaultConnection"]).Returns(expectedConnectionString);

            // Act
            var context = new DapperContext(mockConfig.Object);

            // Assert
            Assert.NotNull(context);
        }

        [Fact(DisplayName = "CreateConnection deve retornar SqlConnection com a connection string correta")]
        public void CreateConnection_ShouldReturnSqlConnection_WithCorrectConnectionString()
        {
            // Arrange
            var expectedConnectionString = "Server=localhost;Database=TestDb;User Id=sa;Password=123;";
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ConnectionStrings:DefaultConnection"]).Returns(expectedConnectionString);

            var context = new DapperContext(mockConfig.Object);

            // Act
            IDbConnection connection = context.CreateConnection();

            // Assert
            Assert.NotNull(connection);
            Assert.IsType<SqlConnection>(connection);
            Assert.Equal(expectedConnectionString, connection.ConnectionString);
        }

        [Fact(DisplayName = "CreateConnection deve lançar exceção se a connection string for nula")]
        public void CreateConnection_ShouldThrowException_WhenConnectionStringIsNull()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ConnectionStrings:DefaultConnection"]).Returns((string)null);

            var context = new DapperContext(mockConfig.Object);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => context.CreateConnection());
            Assert.Contains("Connection string não pode ser nula ou vazia", exception.Message);
        }

        [Fact(DisplayName = "CreateConnection deve lançar exceção se a connection string for vazia")]
        public void CreateConnection_ShouldThrowException_WhenConnectionStringIsEmpty()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["ConnectionStrings:DefaultConnection"]).Returns(string.Empty);

            var context = new DapperContext(mockConfig.Object);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => context.CreateConnection());
            Assert.Contains("Connection string não pode ser nula ou vazia", exception.Message);
        }
    }
}
