using DMS.Application;
using DMS.Domain.Settings;
using DMS.Infrastructure;
using DMS.WebAPI.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Information("Starting DMS API up");

builder.Host.UseSerilog(SerilogExtensions.Configuration);

EmailOptions emailSettings = builder.Services.GetOptions<EmailOptions>(EmailOptions.Key);
builder.Services.TryAddSingleton(emailSettings);

string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;

HangfireSettings hangfireSettings = builder.Services.GetOptions<HangfireSettings>(HangfireSettings.Key);
MinioStorage minioStorage = builder.Services.GetOptions<MinioStorage>(MinioStorage.Key);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    databaseConnectionString,
    hangfireSettings,
    minioStorage);

builder.Services.AddDMSServices();

builder.Services.AddHealthChecks()
    .AddSqlServer(databaseConnectionString);

WebApplication app = builder.Build();

app.UseInfrastructure(hangfireSettings);

app.Run();
