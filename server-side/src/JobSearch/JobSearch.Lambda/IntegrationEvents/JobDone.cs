using System.Text.Json.Serialization;

namespace JobSearch.Lambda.IntegrationEvents;

public class JobDone
{
    public Guid AggregateId { get; set; }

    [JsonConstructor]
    public JobDone(Guid aggregateId)
    {
        AggregateId = aggregateId;
    }
}
