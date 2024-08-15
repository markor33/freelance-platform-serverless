using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FeedbackManagement.Persistence;

namespace FeedbackManagement.Lambda.Handlers;

public class ContractFinishedHandler
{
    private readonly IFeedbackRepository _repository;

    public ContractFinishedHandler()
    {
        _repository = new FeedbackRepository();
    }

    public async Task FunctionHandler(EventBusEvent<ContractFinishedIntegrationEvent> @event, ILambdaContext context)
    {
        var finishedContract = new FinishedContract(@event.Detail.ContractId, @event.Detail.JobId, @event.Detail.ClientId, @event.Detail.FreelancerId);
        await _repository.SaveAsync(finishedContract);
    }

}

public record ContractFinishedIntegrationEvent
{
    public Guid ContractId { get; set; }
    public Guid JobId { get; set; }
    public Guid ClientId { get; set; }
    public Guid FreelancerId { get; set; }
}
