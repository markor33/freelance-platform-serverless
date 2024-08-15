using System.Text.Json.Serialization;

namespace JobManagement.Application.IntegrationEvents.Events
{
    public record ProposalCreatedIntegrationEvent 
    {
        public Guid FreelancerId { get; init; }
        public Guid JobId { get; init; }
        public Guid ProposalId { get; init; }
        public int PriceInCredits { get; init; }

        public ProposalCreatedIntegrationEvent()
        {
        }

        [JsonConstructor]
        public ProposalCreatedIntegrationEvent(Guid freelancerId, Guid jobId, Guid proposalId, int priceInCredits)
        {
            FreelancerId = freelancerId;
            JobId = jobId;
            ProposalId = proposalId;
            PriceInCredits = priceInCredits;
        }

    }
}
