using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class EducationUpdatedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<EducationUpdated> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);
        var educationToUpdate = freelancerViewModel.Educations.Find(e => e.Id == detail.EducationId);
        
        if (educationToUpdate == null)
        {
            context.Logger.LogError($"Education ReadModel sync failed - education with {detail.EducationId} does not exist");
            return;
        }

        educationToUpdate.SchoolName = detail.SchoolName;
        educationToUpdate.Degree = detail.Degree;
        educationToUpdate.Attended = detail.Attended;
        await _repository.SaveAsync(freelancerViewModel);
    }

}
