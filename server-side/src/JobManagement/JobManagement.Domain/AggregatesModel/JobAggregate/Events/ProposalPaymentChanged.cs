using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class ProposalPaymentChanged : DomainEvent
    {
        public Guid ProposalId { get; private set; }
        public Payment Payment { get; private set; }

        [JsonConstructor]
        public ProposalPaymentChanged(Guid aggregateId, Guid proposalId, Payment payment) : base(aggregateId)
        {
            ProposalId = proposalId;
            Payment = payment;
        }
    }
}
