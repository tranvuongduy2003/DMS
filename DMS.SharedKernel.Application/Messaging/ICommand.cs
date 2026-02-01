using DMS.SharedKernel.Domain;
using MediatR;

namespace DMS.SharedKernel.Application.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;
