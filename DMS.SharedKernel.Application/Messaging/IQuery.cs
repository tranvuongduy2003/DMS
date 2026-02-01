using DMS.SharedKernel.Domain;
using MediatR;

namespace DMS.SharedKernel.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
