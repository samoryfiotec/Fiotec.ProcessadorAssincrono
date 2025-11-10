using System.Data;
using System.Threading.Channels;
using Fiotec.ProcessadorAssincrono.Application.Interfaces;
using Fiotec.ProcessadorAssincrono.Domain.DTOs;
using Fiotec.ProcessadorAssincrono.Domain.Entities;
using Fiotec.ProcessadorAssincrono.Infrastructure.BackgroundServices;
using Fiotec.ProcessadorAssincrono.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IAprovacaoService, AprovacaoService>();
builder.Services.AddSingleton<IProcessadorQueue, ProcessadorQueueService>();
builder.Services.AddHostedService<ProcessadorQueueService>();

builder.Services.AddSingleton(Channel.CreateUnbounded<AprovacaoDiariaMessage>());
builder.Services.AddScoped<IAprovacaoService, AprovacaoService>();
//builder.Services.AddHostedService<ProcessadorAssincronoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/home", () => "ProcessadorAssincrono!");

app.MapPut("/api/diarias/{id:guid}/aprovar", async (
    Guid id,
    AprovacaoDiariaRequest request,
    Channel<AprovacaoDiariaMessage> channel) =>
{
    var mensagem = new AprovacaoDiariaMessage
    {
        Id = id,
        Pep = request.Pep,
        ComentariosAdicionais = request.ComentariosAdicionais
    };

    await channel.Writer.WriteAsync(mensagem);

    return Results.Accepted($"/api/diarias/{id}/aprovar", new
    {
        mensagem = $"Diária {id} enfileirada para aprovação."
    });
});

app.MapPost("/aprovar-em-lote", async (List<Guid> ids, IProcessadorQueue queue) =>
{
    foreach (var id in ids)
    {
        await queue.EnfileirarAsync(id);
    }

    return Results.Accepted();
});

app.Run();
