using FluentResults;
using MediatR;
using System.Text.Json.Serialization;

namespace JobManagement.Application.Commands.ContractCommands
{
    public class MakeContractCommand : IRequest<Result>
    {
        public Guid JobId { get; private set; }
        public Guid ProposalId { get; private set; }

        [JsonConstructor]
        public MakeContractCommand(Guid jobId, Guid proposalId)
        {
            JobId = jobId;
            ProposalId = proposalId;
        }
    }
}
