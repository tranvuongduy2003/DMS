using System.Globalization;
using Serilog;

namespace DMS.WebAPI.Extensions;

internal static class SerilogExtensions
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configuration => (context, configuration) =>
    {
        string environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";
        string serverUrl = context.Configuration.GetValue<string>("SeqConfiguration:ServerUrl") ?? "";

        configuration
            .WriteTo.Debug(formatProvider: CultureInfo.InvariantCulture)
            .WriteTo.Seq(serverUrl, formatProvider: CultureInfo.InvariantCulture)
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", formatProvider: CultureInfo.InvariantCulture)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", environmentName)
            .ReadFrom.Configuration(context.Configuration);
    };
}
