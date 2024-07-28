using FluentResults;
using MediatR;
using System.Text.Json.Serialization;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class ApproveProposalCommand : IRequest<Result>
    {
        public Guid JobId { get; private set; }
        public Guid ProposalId { get; private set; }

        [JsonConstructor]
        public ApproveProposalCommand(Guid jobId, Guid proposalId)
        {
            JobId = jobId;
            ProposalId = proposalId;
        }

    }
}
