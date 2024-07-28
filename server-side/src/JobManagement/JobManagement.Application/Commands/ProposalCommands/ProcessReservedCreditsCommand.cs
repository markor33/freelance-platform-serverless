using MediatR;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class ProcessReservedCreditsCommand : IRequest<Unit>
    {
        public Guid JobId { get; private set; }
        public Guid ProposalId { get; private set; }

        public ProcessReservedCreditsCommand(Guid jobId, Guid proposalId)
        {
            JobId = jobId;
            ProposalId = proposalId;
        }
    }
}
