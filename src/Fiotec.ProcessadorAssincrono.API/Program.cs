using System.Data;
using Fiotec.ProcessadorAssincrono.Application.Interfaces;
using Fiotec.ProcessadorAssincrono.Infrastructure.BackgroundServices;
using Fiotec.ProcessadorAssincrono.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAprovacaoService, AprovacaoService>();
builder.Services.AddSingleton<IProcessadorQueue, ProcessadorQueueService>();
builder.Services.AddHostedService<ProcessadorQueueService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/home", () => "ProcessadorAssincrono!");

app.MapPost("/aprovar-em-lote", async (List<Guid> ids, IProcessadorQueue queue) =>
{
    foreach (var id in ids)
    {
        await queue.EnfileirarAsync(id);
    }

    return Results.Accepted();
});

app.Run();
