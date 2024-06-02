namespace FreelancerProfile.Domain.SeedWork
{
    public abstract class DomainEvent(Guid aggregateId)
    {
        public Guid AggregateId { get; private set; } = aggregateId;
    }
}
