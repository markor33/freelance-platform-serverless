namespace JobManagement.Domain.SeedWork
{
    public abstract class EventSourcedAggregate : Entity<Guid>, IAggregateRoot
    {
        public List<DomainEvent> Changes { get; private set; }
        public int Version { get; protected set; } = 0;

        public EventSourcedAggregate()
        {
            Changes = new List<DomainEvent>();
        }

        public abstract void Apply(DomainEvent @event);

        public void ClearDomainEvents()
        {
            Changes.Clear();
        }
    }
}
