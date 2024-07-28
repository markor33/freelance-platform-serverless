using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class ProposalStatusChangedHandler
{
    private readonly IJobReadModelRepository _jobRepository;
    private readonly IProposalReadModelRepository _proposalRepository;

    public ProposalStatusChangedHandler()
    {
        _jobRepository = new JobReadModelRepository();
        _proposalRepository = new ProposalReadModelRepository();
    }

    public async Task FunctionHandler(EventBusEvent<ProposalStatusChanged> @event, ILambdaContext context)
    {
        var detail = @event.Detail;
        var job = await _jobRepository.GetByIdAsync(detail.AggregateId);

        if (detail.Status == ProposalStatus.SENT)
        {
            job.NumOfProposals += 1;
        }

        var proposal = await _proposalRepository.GetById(detail.ProposalId);
        proposal.Status = detail.Status;

        await _jobRepository.SaveAsync(job);
        await _proposalRepository.SaveAsync(proposal);
    }

}
