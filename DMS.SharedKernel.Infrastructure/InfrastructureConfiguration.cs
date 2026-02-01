using Dapper;
using DMS.SharedKernel.Application.Caching;
using DMS.SharedKernel.Application.Data;
using DMS.SharedKernel.Infrastructure.Caching;
using DMS.SharedKernel.Infrastructure.Data;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace DMS.SharedKernel.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IHostApplicationBuilder AddInfrastructureBuilder(
        this IHostApplicationBuilder builder,
        string databaseConnectionName,
        string redisConnectionName)
    {
        builder.AddRedisClient(redisConnectionName);
        builder.AddRedisDistributedCache(redisConnectionName);
        
        builder.AddNpgsqlDataSource(databaseConnectionName);
        
        return builder;
    }
    
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        SqlMapper.AddTypeHandler(new GenericArrayHandler<string>());
        
        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1024 * 1024; // 1 MB
            options.MaximumKeyLength = 512;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
                LocalCacheExpiration = TimeSpan.FromMinutes(1)
            };
        });
        
        services.TryAddSingleton<ICacheService, CacheService>();
        
        return services;
    }
}
