using System.Diagnostics;
using DMS.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DMS.Application.Behaviours;

public sealed class RequestLoggingPipelineBehaviour<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string contextName = GetContextName(typeof(TRequest).FullName!);
        string requestName = typeof(TRequest).Name;

        Activity.Current?.SetTag("request.context", contextName);
        Activity.Current?.SetTag("request.name", requestName);

        using (LogContext.PushProperty("Context", contextName))
        {
            logger.LogInformation("Processing request {RequestName}", requestName);

            TResponse result = await next();

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed request {RequestName}", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed request {RequestName} with error", requestName);
                }
            }

            return result;
        }
    }

    private static string GetContextName(string requestName) => requestName.Split('.')[3];
}
