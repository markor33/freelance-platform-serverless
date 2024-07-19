using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class EmploymentAddedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<EmploymentAdded> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);
        var employmentViewModel = EmploymentViewModel.FromEmployment(detail.Employment);
        freelancerViewModel.Employments.Add(employmentViewModel);

        await _repository.SaveAsync(freelancerViewModel);
    }
}
