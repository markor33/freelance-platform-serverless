using FluentResults;
using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class ApproveProposalCommandHandler : IRequestHandler<ApproveProposalCommand, Result>
    {
        private readonly IJobRepository _jobRepository;

        public ApproveProposalCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Result> Handle(ApproveProposalCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job is null)
                return Result.Fail("Job does not exist");

            job.SetProposalStatusToClientApproved(request.ProposalId);

            await _jobRepository.SaveAsync(job);

            return Result.Ok();
        }
    }
}
