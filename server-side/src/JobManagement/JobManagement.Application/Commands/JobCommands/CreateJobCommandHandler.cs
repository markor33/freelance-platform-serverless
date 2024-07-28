using FluentResults;
using JobManagement.Domain.AggregatesModel.JobAggregate;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.Repositories;
using MediatR;

namespace JobManagement.Application.Commands.JobCommands
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Result<Job>>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IProfessionRepository _professionRepository;

        public CreateJobCommandHandler(
            IJobRepository jobRepository,
            IProfessionRepository professionRepository)
        {
            _jobRepository = jobRepository;
            _professionRepository = professionRepository;
        }

        public async Task<Result<Job>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var profession = await _professionRepository.GetByIdAsync(request.ProfessionId);
            if (profession is null)
                return Result.Fail($"Profession with '{request.ProfessionId}' id, does not exist");

            profession.TryGetSkills(request.Skills, out List<Skill> skills);
            var job = Job.Create(request.ClientId, request.Title, request.Description, request.ExperienceLevel, 
                request.Payment, profession, request.Questions, skills);

            await _jobRepository.SaveAsync(job);

            return Result.Ok(job);
        }

    }
}
