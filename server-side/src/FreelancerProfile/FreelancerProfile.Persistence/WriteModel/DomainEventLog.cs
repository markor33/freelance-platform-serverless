using Amazon.DynamoDBv2.DataModel;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json;

namespace WriteModel
{
    [DynamoDBTable("FPEventStore")]
    public class DomainEventLog
    {
        [DynamoDBRangeKey]
        public string EventId { get; private set; } = Guid.NewGuid().ToString();
        [DynamoDBHashKey]
        public string AggregateId { get; private set; }
        public string EventType { get; private set; }
        public string EventData { get; private set; }
        public DateTime Created { get; private set; }

        public DomainEventLog()
        {

        }

        public DomainEventLog(DomainEvent @event)
        {
            EventId = Guid.NewGuid().ToString();
            AggregateId = @event.AggregateId.ToString();
            EventType = @event.GetType().AssemblyQualifiedName;
            EventData = JsonSerializer.Serialize(@event, @event.GetType());
            Created = DateTime.UtcNow;
        }
    }
}
