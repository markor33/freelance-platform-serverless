using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class CertificationDeletedHandler
{
    private readonly IFreelancerReadModelRepository _repository = new FreelancerReadModelRepository();

    public async Task FunctionHandler(EventBusEvent<CertificationDeleted> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        var freelancerViewModel = await _repository.GetByIdAsync(@event.Detail.AggregateId);
        var certificationViewModel = freelancerViewModel.Certifications.Find(x => x.Id == detail.CertificationId);

        if (certificationViewModel == null)
        {
            context.Logger.LogError($"Certification ReadModel sync failed - certification with {detail.CertificationId} does not exist");
            return;
        }

        freelancerViewModel.Certifications.Remove(certificationViewModel);

        await _repository.SaveAsync(freelancerViewModel);
    }
}
