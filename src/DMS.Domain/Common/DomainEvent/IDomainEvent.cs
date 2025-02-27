using MediatR;

namespace DMS.Domain.Common.DomainEvent;

public interface IDomainEvent : INotification
{
    Guid Id { get; init; }

    DateTime OccurredOnUtc { get; }
}
