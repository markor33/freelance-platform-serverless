using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class JobCreatedHandler
{
    private readonly IJobReadModelRepository _repository;

    public JobCreatedHandler()
    {
        _repository = new JobReadModelRepository();
    }

    public async Task FunctionHandler(EventBusEvent<JobCreated> @event, ILambdaContext context)
    {
        var detail = @event.Detail;
        var questions = detail.Questions.Select(x => new QuestionViewModel(x.Id, x.Text)).ToList();
        var skills = SkillViewModel.FromSkills(detail.Skills);
        var profession = new ProfessionViewModel()
        {
            Id = detail.ProfessionId
        };
        var jobViewModel = new JobViewModel(detail.AggregateId, detail.ClientId, detail.Title, detail.Description, detail.Created, 
            detail.ExperienceLevel, detail.Status, detail.Payment, detail.Credits, questions, profession, skills, 0, 0, 0, 0);

        await _repository.SaveAsync(jobViewModel);
    }
}
