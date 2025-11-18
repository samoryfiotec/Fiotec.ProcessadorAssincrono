using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiotec.ProcessadorAssincrono.Tests.Application
{

    public class AprovacaoServiceTests
    {
        //[Fact]
        //public async Task AprovarAsync_DeveExecutarUpdate()
        //{
        //    var mockConnection = new Mock<IDbConnection>();
        //    mockConnection.Setup(c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
        //                  .ReturnsAsync(1);

        //    var mockLogger = new Mock<ILogger<AprovacaoService>>();

        //    var service = new AprovacaoService(mockConnection.Object, mockLogger.Object);
        //    var id = Guid.NewGuid();

        //    await service.AprovarAsync(id, "", "");

        //    mockConnection.Verify(c => c.ExecuteAsync(
        //        "UPDATE Requisicoes SET Aprovada = 1 WHERE Id = @Id AND Aprovada = 0",
        //        It.Is<object>(o => o != null && (Guid)o.GetType().GetProperty("Id").GetValue(o) == id),
        //        null, null, null), Times.Once);
        //}
    }

}
