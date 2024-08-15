using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class ProposalCreated : DomainEvent
    {
        public Proposal Proposal { get; private set; }
        public int Credits { get; private set; }

        [JsonConstructor]
        public ProposalCreated(Guid aggregateId, Proposal proposal, int credits) : base(aggregateId)
        {
            Proposal = proposal;
            Credits = credits;
        }
    }
}
