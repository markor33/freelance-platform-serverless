using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class ProfileSummaryUpdatedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<ProfileSummaryUpdated> @event, ILambdaContext context)
    {
        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);

        freelancerViewModel.ProfileSummary =  @event.Detail.ProfileSummary;

        await _repository.SaveAsync(freelancerViewModel);
    }
}
