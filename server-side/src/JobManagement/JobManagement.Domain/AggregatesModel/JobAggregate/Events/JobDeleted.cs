using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class JobDeleted : DomainEvent
    {
        [JsonConstructor]
        public JobDeleted(Guid aggregateId) : base(aggregateId) { } 
    }
}
