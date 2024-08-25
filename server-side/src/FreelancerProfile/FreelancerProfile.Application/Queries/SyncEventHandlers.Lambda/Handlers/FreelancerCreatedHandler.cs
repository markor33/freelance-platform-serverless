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
            Id = @event.Detail.UserId,
            FirstName = @event.Detail.FirstName,
            LastName = @event.Detail.LastName,
            Contact = @event.Detail.Contact,
            Joined = @event.Detail.Joined,
        };

        await _repository.SaveAsync(freelancerViewModel);
    }
}

