using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class ProfileSetupCompletedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<ProfileSetupCompleted> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);

        freelancerViewModel.IsProfilePublic = detail.IsProfilePublic;
        freelancerViewModel.ProfileSummary = detail.ProfileSummary;
        freelancerViewModel.HourlyRate = detail.HourlyRate;
        freelancerViewModel.Availability = detail.Availability;
        freelancerViewModel.ExperienceLevel = detail.ExperienceLevel;
        freelancerViewModel.Profession = new ProfessionViewModel()
        {
            Id = detail.ProfessionId
        };
        freelancerViewModel.LanguageKnowledges.Add(detail.LanguageKnowledge);

        await _repository.SaveAsync(freelancerViewModel);
    }
}
