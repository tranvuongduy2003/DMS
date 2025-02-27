using DMS.Domain.Settings;
using DMS.WebAPI.Middlewares;
using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace DMS.WebAPI.Extensions;

internal static class ApplicationExtensions
{
    public static WebApplication UseInfrastructure(this WebApplication app, HangfireSettings hangfireSettings)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.ApplyMigrations();
        }

        app.MapHealthChecks("health", new HealthCheckOptions()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseLogContext();

        app.UseSerilogRequestLogging();

        app.UseExceptionHandler();

        app.UseCors(CorsPolicy.Name);

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseHangfireDashboard(hangfireSettings.Route, new DashboardOptions
        {
            // Authorization = new [] { },
            DashboardTitle = hangfireSettings.Dashboard.DashboardTitle,
            StatsPollingInterval = hangfireSettings.Dashboard.StatsPollingInterval,
            AppPath = hangfireSettings.Dashboard.AppPath,
            IgnoreAntiforgeryToken = true,
        });

        app.MapEndpoints();

        return app;
    }
}
