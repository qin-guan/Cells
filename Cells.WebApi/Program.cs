using Cells.WebApi.Grains.Cell;
using Cells.WebApi.Grains.Dataset;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddKeyedAzureTableClient("clustering");

builder.AddKeyedAzureBlobClient("dataset");
builder.AddKeyedAzureBlobClient("column");
builder.AddKeyedAzureBlobClient("cell");

builder.UseOrleans(orleans => { orleans.UseDashboard(o => o.Port = 18080); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/{datasetId:guid}", async (
        [FromRoute] Guid datasetId,
        IGrainFactory grainFactory
    ) =>
    {
        var datasetGrain = grainFactory.GetGrain<IDatasetGrain>(datasetId);
        var state = await datasetGrain.GetAsync();

        var d = new List<string>();

        foreach (var col in state.ColNames)
        {
            for (var row = 0; row < state.Rows; row++)
            {
                var cellGrain = grainFactory.GetGrain<ICellGrain>($"{datasetId:N}-{col}-{row}");
                d.Add(await cellGrain.GetAsync());
            }
        }

        return d;
    })
    .WithOpenApi();

app.MapPost("/", async (
        [FromQuery] Uri uri,
        IGrainFactory grainFactory
    ) =>
    {
        var http = new HttpClient();
        var data = await http.GetStringAsync(uri);

        var datasetId = Guid.NewGuid();
        var datasetGrain = grainFactory.GetGrain<IDatasetGrain>(datasetId);
        await datasetGrain.CreateAsync(data);

        return datasetId;
    })
    .WithOpenApi();

app.Run();