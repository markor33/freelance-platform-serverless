using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class SkillsUpdatedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<SkillsUpdated> @event, ILambdaContext context)
    {
        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);

        freelancerViewModel.Skills = SkillViewModel.FromSkills(@event.Detail.Skills);

        await _repository.SaveAsync(freelancerViewModel);
    }

}
