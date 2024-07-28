using System.Text.Json.Serialization;

namespace JobManagement.Application.Notifications
{
    public record ContractMadeNotification 
    {
        public Guid ContractId { get; private init; }
        public Guid JobId { get; private init; }
        public string JobTitle { get; private init; }
        public Guid ProposalId { get; private init; }
        public Guid ClientId { get; private init; }

        public ContractMadeNotification()
        {
        }

        [JsonConstructor]
        public ContractMadeNotification(Guid contractId, Guid jobId, string jobTitle, Guid proposalId, Guid clientId)
        {
            ContractId = contractId;
            JobId = jobId;
            JobTitle = jobTitle;
            ProposalId = proposalId;
            ClientId = clientId;
        }

    }
}
