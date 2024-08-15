using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;
using ReadModel;

namespace SyncEventHandlers.Lambda.Handlers;

public class ProposalDeletedHandler
{
    private readonly IProposalReadModelRepository _repository;
    private readonly IJobReadModelRepository _jobRepository;

    public ProposalDeletedHandler()
    {
        _repository = new ProposalReadModelRepository();
    }

    public async Task FunctionHandler(EventBusEvent<ProposalRemoved> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        await _repository.DeleteAsync(detail.ProposalId);

        var job = await _jobRepository.GetByIdAsync(detail.AggregateId);
        await _jobRepository.SaveAsync(job);
    }
}
