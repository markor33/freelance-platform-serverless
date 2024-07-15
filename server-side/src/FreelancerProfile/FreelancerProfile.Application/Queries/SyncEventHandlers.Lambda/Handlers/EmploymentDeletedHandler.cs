using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class EmploymentDeletedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<EmploymentDeleted> @event, ILambdaContext context)
    {

        var detail = @event.Detail;

        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);
        var employmentViewModel = freelancerViewModel.Employments.Find(x => x.Id == detail.EmploymentId);

        if (employmentViewModel == null)
        {
            context.Logger.LogError($"Employment ReadModel sync failed - employment with {detail.EmploymentId} does not exist");
            return;
        }

        freelancerViewModel.Employments.Remove(employmentViewModel);

        await _repository.SaveAsync(freelancerViewModel);
    }
}
