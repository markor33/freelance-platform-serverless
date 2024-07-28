using MediatR;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class ProcessInitialMessageSentCommand : IRequest<Unit>
    {
        public Guid JobId { get; private set; }
        public Guid ProposalId { get; private set; }
        public Guid FreelancerId { get; private set; }

        public ProcessInitialMessageSentCommand(Guid jobId, Guid proposalId, Guid freelancerId)
        {
            JobId = jobId;
            ProposalId = proposalId;
            FreelancerId = freelancerId;
        }
    }
}
