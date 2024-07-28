using FluentResults;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class CreateProposalCommandHandler : IRequestHandler<CreateProposalCommand, Result<Proposal>>
    {
        private readonly IJobRepository _jobRepository;

        public CreateProposalCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Result<Proposal>> Handle(CreateProposalCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job is null)
                return Result.Fail("Job does not exist");

            var proposal = Proposal.Create(request.FreelancerId, request.Text, request.Payment, request.Answers);

            var addProposalResult = job.AddProposal(proposal);
            if (addProposalResult.IsFailed)
                return addProposalResult;

            var result = await _jobRepository.SaveAsync(job);

            return Result.Ok(proposal);
        }

    }
}
