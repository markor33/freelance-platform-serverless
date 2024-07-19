using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class EducationDeletedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<EducationDeleted> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);
        var educationViewModel = freelancerViewModel.Educations.Find(x => x.Id == detail.EducationId);
        
        if (educationViewModel == null)
        {
            context.Logger.LogError($"Education ReadModel sync failed - education with {detail.EducationId} does not exist");
            return;
        }

        freelancerViewModel.Educations.Remove(educationViewModel);

        await _repository.SaveAsync(freelancerViewModel);
    }
}
