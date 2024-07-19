using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class CertificationUpdatedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<CertificationUpdated> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);
        var certificationToUpdate = freelancerViewModel.Certifications.Find(e => e.Id == detail.CertificationId);

        if (certificationToUpdate == null)
        {
            context.Logger.LogError($"Certification ReadModel sync failed - certification with {detail.CertificationId} does not exist");
            return;
        }

        certificationToUpdate.Name = detail.Name;
        certificationToUpdate.Provider = detail.Provider;
        certificationToUpdate.Attended = detail.Attended;
        certificationToUpdate.Description = detail.Description;

        await _repository.SaveAsync(freelancerViewModel);
    }
}
