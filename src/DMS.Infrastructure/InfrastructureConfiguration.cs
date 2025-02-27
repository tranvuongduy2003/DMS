using System.Security.Authentication;
using DMS.Domain.Settings;
using DMS.Domain.Users;
using DMS.Domain.Users.Entities;
using DMS.Infrastructure.Authentication;
using DMS.Infrastructure.Authorization;
using DMS.Infrastructure.Persistance;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace DMS.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string databaseConnectionString,
        HangfireSettings hangfireSettings,
        MinioStorage minioStorage,
        JwtOptions jwtOptions)
    {
        services.AddAuthenticationInternal(jwtOptions);

        services.AddAuthorizationInternal();

        services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.User.RequireUniqueEmail = true;
        });

        services.AddSingleton<DateTrackingInterceptor>();
        services.AddDbContext<DMSDbContext>((provider, optionsBuilder) =>
        {
            DateTrackingInterceptor dateTrackingInterceptor =
                provider.GetService<DateTrackingInterceptor>()!;

            optionsBuilder
                .UseSqlServer(databaseConnectionString, builder => builder.MigrationsAssembly(AssemblyReference.Assembly))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(dateTrackingInterceptor);
        });

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<DMSDbContext>()
            .AddDefaultTokenProviders();

        var mongoUrlBuilder = new MongoUrlBuilder(hangfireSettings.Storage.ConnectionString);
        var mongoClientSettings = MongoClientSettings
            .FromUrl(new MongoUrl(hangfireSettings.Storage.ConnectionString));
        mongoClientSettings.SslSettings = new SslSettings
        {
            EnabledSslProtocols = SslProtocols.None
        };
        using var mongoClient = new MongoClient(mongoClientSettings);
        var mongoStorageOptions = new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            CheckConnection = true,
            Prefix = "SchedulerQueue",
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
        };

        services.AddHangfire((provider, config) =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, mongoStorageOptions);

            var jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            config.UseSerializerSettings(jsonSettings);
        });
        services.AddHangfireConsoleExtensions();
        services.AddHangfireServer(options => { options.ServerName = hangfireSettings.ServerName; });

        services.AddMinio(configureClient => configureClient
            .WithEndpoint(minioStorage.Endpoint)
            .WithCredentials(minioStorage.AccessKey, minioStorage.SecretKey)
            .WithSSL(minioStorage.Secure)
            .Build());

        return services;
    }
}
