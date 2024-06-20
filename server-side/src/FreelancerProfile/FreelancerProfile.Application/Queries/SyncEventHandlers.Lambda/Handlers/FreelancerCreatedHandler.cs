using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class FreelancerCreatedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<FreelancerCreated> @event, ILambdaContext context)
    {
        var freelancerViewModel = new FreelancerViewModel()
        {
            Id = @event.Detail.UserId
        };

        await _repository.SaveAsync(freelancerViewModel);
    }
}

