using CoffeeNChill.Functions.Properties.Interfaces;
using CoffeeNChill.Functions.Properties.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var connectionString = builder.Configuration["AzureWebJobsStorage"] ?? "UseDevelopmentStorage=true";
builder.Services.AddSingleton<ITableStorageService>(_ => new TableStorageService(connectionString));

builder.Build().Run();