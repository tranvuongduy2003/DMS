using System.Text;
using DMS.Domain.Settings;
using DMS.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace DMS.WebAPI.Extensions;

internal static class ServiceExtensions
{
    public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
        IConfigurationSection section = configuration.GetSection(sectionName);
        var options = new T();
        section.Bind(options);

        return options;
    }

    public static IServiceCollection AddDMSServices(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(8);
        });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
        });
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy.Name, build =>
            {
                build
                    .WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerDocumentation();

        services.AddEndpoints(AssemblyReference.Assembly);

        JwtOptions jwtOptions = services.GetOptions<JwtOptions>(JwtOptions.Key);
        services.TryAddSingleton(jwtOptions);
        byte[] key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                x.SaveToken = true;
            });

        return services;
    }
}
