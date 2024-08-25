using JobSearch.IndexModel;
using System.Text.Json.Serialization;

namespace JobSearch.Lambda.IntegrationEvents;

public class JobStatusChanged
{
    public Guid AggregateId { get; set; }
    public JobStatus Status { get; set; }

    [JsonConstructor]
    public JobStatusChanged(Guid aggregateId, JobStatus status)
    {
        AggregateId = aggregateId;
        Status = status;
    }
}
