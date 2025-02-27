using MediatR;
using Microsoft.Extensions.Logging;

namespace DMS.Application.Behaviours;

public sealed class ExceptionHandlingPipelineBehaviour<TRequest, TResponse>(
    ILogger<ExceptionHandlingPipelineBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception for {RequestName}", typeof(TRequest).Name);

            throw new ApplicationException(typeof(TRequest).Name, innerException: exception);
        }
    }
}
