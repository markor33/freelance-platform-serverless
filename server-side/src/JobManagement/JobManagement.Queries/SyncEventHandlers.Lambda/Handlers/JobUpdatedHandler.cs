using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class JobUpdatedHandler
{
    private readonly IJobReadModelRepository _repository;

    public JobUpdatedHandler()
    {
        _repository = new JobReadModelRepository();
    }

    public async Task FunctionHandler(EventBusEvent<JobUpdated> @event, ILambdaContext context)
    {
        var detail = @event.Detail;
        var job = await _repository.GetByIdAsync(detail.AggregateId);

        var questions = detail.Questions.Select(x => new QuestionViewModel(x.Id, x.Text)).ToList();
        var skills = SkillViewModel.FromSkills(detail.Skills);
        var profession = new ProfessionViewModel()
        {
            Id = detail.ProfessionId
        };

        job.Title = detail.Title;
        job.Description = detail.Description;
        job.Credits = detail.Credits;
        job.ExperienceLevel = detail.ExperienceLevel;
        job.Payment = detail.Payment;
        job.Questions = questions;
        job.Profession = profession;
        job.Skills = skills;

        await _repository.SaveAsync(job);
    }
}
