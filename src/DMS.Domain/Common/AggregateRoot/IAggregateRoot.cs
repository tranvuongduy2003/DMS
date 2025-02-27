using DMS.Domain.Common.DomainEvent;

namespace DMS.Domain.Common.AggregateRoot;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();

    void ClearDomainEvents();

    void RaiseDomainEvent(IDomainEvent domainEvent);
}
