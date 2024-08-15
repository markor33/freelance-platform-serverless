using FluentResults;
using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class UpdateProposalPaymentCommandHandler : IRequestHandler<UpdateProposalPaymentCommand, Result>
    {
        private readonly IJobRepository _jobRepository;

        public UpdateProposalPaymentCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Result> Handle(UpdateProposalPaymentCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job is null)
                return Result.Fail("Job does not exist");

            job.ChangeProposalPayment(request.ProposalId, request.Payment);
            var proposal = job.GetProposal(request.ProposalId);

            await _jobRepository.SaveAsync(job);

            return Result.Ok();
        }
    }
}
