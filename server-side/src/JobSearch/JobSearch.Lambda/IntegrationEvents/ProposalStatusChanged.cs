using JobSearch.IndexModel;
using System.Text.Json.Serialization;

namespace JobSearch.Lambda.IntegrationEvents;

public class ProposalStatusChanged
{
    public Guid AggregateId { get; set; }
    public Guid ProposalId { get; set; }
    public ProposalStatus Status { get; set; }

    [JsonConstructor]
    public ProposalStatusChanged(Guid aggregateId, Guid proposalId, ProposalStatus status)
    {
        AggregateId = aggregateId;
        ProposalId = proposalId;
        Status = status;
    }
}
