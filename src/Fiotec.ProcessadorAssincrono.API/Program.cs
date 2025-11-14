using System.Data;
using System.Threading.Channels;
using Fiotec.ProcessadorAssincrono.Application.Interfaces;
using Fiotec.ProcessadorAssincrono.Application.Validators;
using Fiotec.ProcessadorAssincrono.Domain.DTOs;
using Fiotec.ProcessadorAssincrono.Domain.Entities;
using Fiotec.ProcessadorAssincrono.Infrastructure.BackgroundServices;
using Fiotec.ProcessadorAssincrono.Infrastructure.Data;
using Fiotec.ProcessadorAssincrono.Infrastructure.Interfaces;
using Fiotec.ProcessadorAssincrono.Infrastructure.Persistence;
using Fiotec.ProcessadorAssincrono.Infrastructure.Services;
using FluentValidation;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IDbConnectionFactory, DapperContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAprovacaoService, AprovacaoService>();

var channel = Channel.CreateUnbounded<Aprovacao>();
builder.Services.AddSingleton(channel);

builder.Services.AddSingleton<ProcessadorQueueService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<ProcessadorQueueService>());
builder.Services.AddSingleton<IProcessadorQueue>(sp => sp.GetRequiredService<ProcessadorQueueService>());

builder.Services.AddScoped<IValidator<AprovacaoRequest>, AprovacaoRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPut("/api/diarias/{id:guid}/aprovar", async (
    Guid id,
    AprovacaoRequest request,
    IProcessadorQueue queue,
    ILogger<Program> logger,
    CancellationToken cancellationToken) =>
{
    await queue.EnfileirarAsync(id, request.Pep, request.ComentariosAdicionais);
    logger.LogInformation("Diária {Id} enfileirada para aprovação.", id);

    return Results.Accepted($"/api/diarias/{id}/aprovar", new
    {
        mensagem = $"Diária {id} enfileirada para aprovação."
    });
});

app.Run();