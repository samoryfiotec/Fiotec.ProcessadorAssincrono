
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Fiotec.ProcessadorAssincrono.Application.Interfaces;
using Fiotec.ProcessadorAssincrono.Domain.DTOs;
using Fiotec.ProcessadorAssincrono.Domain.Entities;
using Fiotec.ProcessadorAssincrono.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Shouldly;
using Xunit;

namespace Fiotec.ProcessadorAssincrono.Tests
{
    public class ProgramEndpointsWithMocksTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProgramEndpointsWithMocksTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove implementações reais
                    services.RemoveAll(typeof(IAprovacaoService));
                    services.RemoveAll(typeof(IProcessadorQueue));
                    services.RemoveAll(typeof(IValidator<LoteAprovacaoRequest>));

                    // Criar mocks
                    var aprovacaoServiceMock = new Mock<IAprovacaoService>();
                    aprovacaoServiceMock
                        .Setup(s => s.InserirAsync(It.IsAny<Aprovacao>()))
                        .Returns(Task.CompletedTask);

                    var processadorQueueMock = new Mock<IProcessadorQueue>();
                    processadorQueueMock
                        .Setup(q => q.EnfileirarAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                        .Returns(Task.CompletedTask);

                    var validatorMock = new Mock<IValidator<LoteAprovacaoRequest>>();
                    validatorMock
                        .Setup(v => v.ValidateAsync(It.IsAny<LoteAprovacaoRequest>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new FluentValidation.Results.ValidationResult());

                    // Registrar mocks
                    services.AddSingleton(aprovacaoServiceMock.Object);
                    services.AddSingleton(processadorQueueMock.Object);
                    services.AddSingleton(validatorMock.Object);
                });
            });
        }
    }
}
