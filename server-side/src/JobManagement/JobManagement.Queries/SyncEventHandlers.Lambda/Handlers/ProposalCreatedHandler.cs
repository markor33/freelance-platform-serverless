using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class ProposalCreatedHandler
{
    private readonly IProposalReadModelRepository _proposalRepository;
    private readonly IJobReadModelRepository _jobRepository;

    public ProposalCreatedHandler()
    {
        _proposalRepository = new ProposalReadModelRepository();
        _jobRepository = new JobReadModelRepository();
    }

    public async Task FunctionHandler(EventBusEvent<ProposalCreated> @event, ILambdaContext context)
    {
        var job = await _jobRepository.GetByIdAsync(@event.Detail.AggregateId);

        var proposal = new ProposalViewModel(@event.Detail.AggregateId, @event.Detail.Proposal, job.Questions.ToDictionary(x => x.Id));

        await _proposalRepository.SaveAsync(proposal);
    }
}
