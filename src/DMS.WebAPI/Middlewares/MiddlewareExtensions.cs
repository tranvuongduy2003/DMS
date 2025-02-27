namespace DMS.WebAPI.Middlewares;

internal static class MiddlewareExtensions
{
    internal static IApplicationBuilder UseLogContext(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogContextTraceLoggingMiddleware>();

        return app;
    }
}
