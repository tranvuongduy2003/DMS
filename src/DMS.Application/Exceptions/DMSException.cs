using DMS.Domain.Common;

namespace DMS.Application.Exceptions;

public sealed class DMSException : Exception
{
    public DMSException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}
