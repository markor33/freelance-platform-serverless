using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class DeleteProposalCommandHandler : IRequestHandler<DeleteProposalCommand, Unit>
    {
        private readonly IJobRepository _jobRepository;

        public DeleteProposalCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Unit> Handle(DeleteProposalCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);

            job.RemoveProposal(request.ProposalId);

            await _jobRepository.SaveAsync(job);

            return Unit.Value;
        }
    }
}
