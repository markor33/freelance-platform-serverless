using FluentResults;
using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.ContractCommands
{
    public class MakeContractCommandHandler : IRequestHandler<MakeContractCommand, Result>
    {
        private readonly IJobRepository _jobRepository;

        public MakeContractCommandHandler(
            IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Result> Handle(MakeContractCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job is null)
                return Result.Fail("Job does not exist");

            var contract = job.MakeContract(request.ProposalId).Value;

            await _jobRepository.SaveAsync(job);

            return Result.Ok();
        }
    }
}
