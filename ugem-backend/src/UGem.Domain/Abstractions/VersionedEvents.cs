namespace UGem.Domain.Abstractions;

public interface IVersionedEvent : IDomainEvent
{
    int Version { get; }
}

public abstract record VersionedIntegrationEvent(
    Guid Id, 
    DateTime OccurredOnUtc, 
    int Version) : IIntegrationEvent
{
    protected VersionedIntegrationEvent(int version) 
        : this(Guid.NewGuid(), DateTime.UtcNow, version) { }
}

public interface IEventMapper
{
    // Strategy for mapping domain events to integration events with versioning
    IIntegrationEvent Map(IDomainEvent domainEvent);
}
