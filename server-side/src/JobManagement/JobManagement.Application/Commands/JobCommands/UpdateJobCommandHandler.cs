using FluentResults;
using JobManagement.Domain.AggregatesModel.JobAggregate;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.JobCommands
{
    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, Result<Job>>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IProfessionRepository _professionRepository;

        public UpdateJobCommandHandler(
            IJobRepository jobRepository, 
            IProfessionRepository professionRepository)
        {
            _jobRepository = jobRepository;
            _professionRepository = professionRepository;
        }

        public async Task<Result<Job>> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job is null)
                return Result.Fail("Job does not exist");

            var profession = await _professionRepository.GetByIdAsync(job.ProfessionId);
            profession.TryGetSkills(request.Skills, out List<Skill> skills);

            job.Update(request.Title, request.Description, request.ExperienceLevel, request.Payment, request.ProfessionId, request.Questions, skills);

            await _jobRepository.SaveAsync(job);

            return Result.Ok(job);
        }

    }
}
