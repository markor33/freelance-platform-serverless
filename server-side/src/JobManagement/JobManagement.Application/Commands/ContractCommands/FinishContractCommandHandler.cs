using FluentResults;
using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.ContractCommands
{
    public class FinishContractCommandHandler : IRequestHandler<FinishContractCommand, Result>
    {
        private readonly IJobRepository _jobRepository;

        public FinishContractCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Result> Handle(FinishContractCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job is null)
                return Result.Fail("Job does not exist");

            var contract = job.FinishContract(request.ContractId);

            await _jobRepository.SaveAsync(job);

            return Result.Ok();
        }
    }
}
