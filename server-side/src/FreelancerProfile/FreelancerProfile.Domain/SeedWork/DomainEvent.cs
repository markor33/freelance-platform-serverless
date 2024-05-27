namespace FreelancerProfile.Domain.SeedWork
{
    public abstract class DomainEvent
    {
        public Guid AggregateId { get; private set; }

        protected DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
