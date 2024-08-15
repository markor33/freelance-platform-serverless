using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class ProposalRemoved : DomainEvent
    {
        public Guid ProposalId { get; private set; }

        [JsonConstructor]
        public ProposalRemoved(Guid aggregateId, Guid proposalId) : base(aggregateId)
        {
            ProposalId = proposalId;
        }
    }
}
