using System.Text.Json.Serialization;

namespace JobManagement.Application.IntegrationEvents.Events
{
    public record CreditsReservedIntegrationEvent
    {
        public Guid JobId { get; init; }
        public Guid ProposalId { get; init; }

        public CreditsReservedIntegrationEvent()
        {
        }

        [JsonConstructor]
        public CreditsReservedIntegrationEvent(Guid jobId, Guid proposalId)
        {
            JobId = jobId;
            ProposalId = proposalId;
        }

    }
}
