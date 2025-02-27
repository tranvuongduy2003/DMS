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
JwtOptions jwtOptions = builder.Services.GetOptions<JwtOptions>(JwtOptions.Key);
builder.Services.TryAddSingleton(jwtOptions);

string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;

HangfireSettings hangfireSettings = builder.Services.GetOptions<HangfireSettings>(HangfireSettings.Key);
MinioStorage minioStorage = builder.Services.GetOptions<MinioStorage>(MinioStorage.Key);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    databaseConnectionString,
    hangfireSettings,
    minioStorage,
    jwtOptions);

builder.Services.AddDMSServices();

builder.Services.AddHealthChecks()
    .AddSqlServer(databaseConnectionString);

builder.Configuration.AddConfiguration();

WebApplication app = builder.Build();

app.UseInfrastructure(hangfireSettings);

app.Run();
