using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class JobDone : DomainEvent
    {
        [JsonConstructor]
        public JobDone(Guid aggregateId) : base(aggregateId) { }
    }
}
