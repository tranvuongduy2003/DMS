using DMS.Domain.Common.DomainEvent;
using DMS.Domain.Common.Entities;

namespace DMS.Domain.Common.AggregateRoot;

public abstract class AggregateRoot : EntityBase, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot()
    {
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}
