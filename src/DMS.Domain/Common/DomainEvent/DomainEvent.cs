namespace DMS.Domain.Common.DomainEvent;

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
    }

    protected DomainEvent(Guid id, DateTime occurredOnUtc)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
    }

    public Guid Id { get; init; }

    public DateTime OccurredOnUtc { get; init; }
}
