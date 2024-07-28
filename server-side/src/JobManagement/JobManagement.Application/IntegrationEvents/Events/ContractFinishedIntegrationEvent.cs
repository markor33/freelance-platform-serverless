using System.Text.Json.Serialization;

namespace JobManagement.Application.IntegrationEvents.Events
{
    public record ContractFinishedIntegrationEvent
    {
        public Guid ContractId { get; private set; }
        public Guid JobId { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid FreelancerId { get; private set; }

        public ContractFinishedIntegrationEvent()
        {
        }

        [JsonConstructor]
        public ContractFinishedIntegrationEvent(Guid contractId, Guid jobId, Guid clientId, Guid freelancerId)
        {
            ContractId = contractId;
            JobId = jobId;
            ClientId = clientId;
            FreelancerId = freelancerId;
        }
    }
}
