using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class ProposalPaymentChangedHandler
{
    private readonly IProposalReadModelRepository _proposalRepository;

    public ProposalPaymentChangedHandler()
    {
        _proposalRepository = new ProposalReadModelRepository();
    }

    public async Task FunctionHandler(EventBusEvent<ProposalPaymentChanged> @event, ILambdaContext context)
    {
        var detail = @event.Detail;

        var proposal = await _proposalRepository.GetById(detail.ProposalId);
        proposal.Payment = detail.Payment;

        await _proposalRepository.SaveAsync(proposal);
    }

}
