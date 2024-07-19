using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class EducationAddedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<EducationAdded> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);
        
        var educationViewModel = EducationViewModel.FromEducation(detail.Education);
        freelancerViewModel.Educations.Add(educationViewModel);

        await _repository.SaveAsync(freelancerViewModel);
    }
}
