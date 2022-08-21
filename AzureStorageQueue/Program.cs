using Azure.Storage.Queues;
using AzureStorageQueue;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHostedService<WeatherDataBackgroundTask>();
builder.Services.AddAzureClients(builder =>
{
    builder.AddClient<QueueClient, QueueClientOptions>((options, _, _) =>
    {
        var connectionString = "DefaultEndpointsProtocol=https;AccountName=learnaboutazurestorage;AccountKey=NWgOM70GM/nR6QaAi9/2At7T3bDTxmZ9oncirAmjsBDpLdIW1i1nd+ee1aB+85ASDEaBvnifl6ed+AStWwbj8g==;EndpointSuffix=core.windows.net";
        var queueName = "add-weatherdata";
        return new QueueClient(connectionString, queueName);
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
