using System.Text.Json.Serialization;

namespace JobSearch.Lambda.IntegrationEvents;

public class JobDeleted
{
    public Guid AggregateId { get; set; }

    [JsonConstructor]
    public JobDeleted(Guid aggregateId)
    {
        AggregateId = aggregateId;
    }
}
