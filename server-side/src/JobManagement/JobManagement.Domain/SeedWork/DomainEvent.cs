namespace JobManagement.Domain.SeedWork
{
    public abstract class DomainEvent
    {
        public Guid AggregateId { get; private set; }

        public DomainEvent() { }

        protected DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
