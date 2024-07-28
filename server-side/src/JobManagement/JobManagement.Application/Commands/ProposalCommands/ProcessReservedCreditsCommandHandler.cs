using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class ProcessReservedCreditsCommandHandler : IRequestHandler<ProcessReservedCreditsCommand, Unit>
    {
        private readonly IJobRepository _jobRepository;

        public ProcessReservedCreditsCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Unit> Handle(ProcessReservedCreditsCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);

            job.SetProposalStatusToSent(request.ProposalId);

            await _jobRepository.SaveAsync(job);

            return Unit.Value;
        }
    }
}
