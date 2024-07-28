using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class ProposalStatusChanged : DomainEvent
    {
        public Guid ProposalId { get; private set; }
        public ProposalStatus Status { get; private set; }

        [JsonConstructor]
        public ProposalStatusChanged(Guid aggregateId, Guid proposalId, ProposalStatus status) : base(aggregateId)
        {
            ProposalId = proposalId;
            Status = status;
        }
    }
}
