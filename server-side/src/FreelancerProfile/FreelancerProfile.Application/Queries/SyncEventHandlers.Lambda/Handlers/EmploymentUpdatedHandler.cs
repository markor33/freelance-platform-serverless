using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class EmploymentUpdatedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<EmploymentUpdated> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);
        var employmentToUpdate = freelancerViewModel.Employments.Find(e => e.Id == detail.EmploymentId);

        if (employmentToUpdate == null)
        {
            context.Logger.LogError($"Employment ReadModel sync failed - employment with {detail.EmploymentId} does not exist");
            return;
        }

        employmentToUpdate.Company = detail.Company;
        employmentToUpdate.Description = detail.Description;
        employmentToUpdate.Title = detail.Title;
        employmentToUpdate.Period = detail.Period;

        await _repository.SaveAsync(freelancerViewModel);
    }
}
