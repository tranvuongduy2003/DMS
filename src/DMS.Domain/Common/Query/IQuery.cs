using MediatR;

namespace DMS.Domain.Common.Query;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
