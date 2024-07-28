using MediatR;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class DeleteProposalCommand : IRequest<Unit>
    {
        public Guid JobId { get; private set; }
        public Guid ProposalId { get; private set; }

        public DeleteProposalCommand(Guid jobId, Guid proposalId)
        {
            JobId = jobId;
            ProposalId = proposalId;
        }

    }
}
