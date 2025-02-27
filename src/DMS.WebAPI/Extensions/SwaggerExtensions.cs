using Microsoft.OpenApi.Models;

namespace DMS.WebAPI.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DMS API",
                Version = "v1",
                Description = "DMS API built using the clean architecture."
            });

            options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
        });

        return services;
    }
}
