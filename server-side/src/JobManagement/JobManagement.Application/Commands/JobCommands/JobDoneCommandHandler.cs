using FluentResults;
using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.JobCommands
{
    public class JobDoneCommandHandler : IRequestHandler<JobDoneCommand, Result>
    {
        private readonly IJobRepository _jobRepository;

        public JobDoneCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Result> Handle(JobDoneCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job is null)
                return Result.Fail("Job does not exist");

            var result = job.Done();
            if (result.IsFailed)
                return Result.Fail(result.Errors);
            await _jobRepository.SaveAsync(job);

            return Result.Ok();
        }
    }
}
