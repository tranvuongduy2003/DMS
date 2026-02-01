using DMS.SharedKernel.Domain;
using MediatR;

namespace DMS.SharedKernel.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
