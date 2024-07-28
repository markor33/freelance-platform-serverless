using JobManagement.Application.IntegrationEvents;
using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class ProcessInitialMessageSentCommandHandler : IRequestHandler<ProcessInitialMessageSentCommand, Unit>
    {
        private readonly IJobRepository _jobRepository;

        public ProcessInitialMessageSentCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Unit> Handle(ProcessInitialMessageSentCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);

            job.SetProposalStatusToInterview(request.ProposalId);

            await _jobRepository.SaveAsync(job);

            return Unit.Value;
        }
    }
}
