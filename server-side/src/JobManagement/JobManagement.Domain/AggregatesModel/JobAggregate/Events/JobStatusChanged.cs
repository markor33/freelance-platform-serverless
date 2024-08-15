using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class JobStatusChanged : DomainEvent
    {
        public JobStatus Status { get; private set; }

        [JsonConstructor]
        public JobStatusChanged(Guid aggregateId, JobStatus status) : base(aggregateId)
        {
            Status = status;
        }
    }
}
