namespace FreelancerProfile.Domain.SeedWork
{
    public abstract class DomainEvent
    {
        public Guid AggregateId { get; set; }

        public DomainEvent() { }

        public DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
