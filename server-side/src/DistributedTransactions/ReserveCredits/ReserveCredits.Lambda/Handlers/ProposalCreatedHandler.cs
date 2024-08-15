using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using FreelancerProfile.Domain.Repositories;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using WriteModel;

namespace ReserveCredits.Lambda.Handlers;

public class ProposalCreatedHandler
{
    private readonly IFreelancerRepository _freelancerRepository;

    public ProposalCreatedHandler()
    {
        _freelancerRepository = new FreelancerRepository();
    }

    public async Task<CreditsSubstractionResult> FunctionHandler(EventBusEvent<ProposalCreated> @event, ILambdaContext context)
    {
        var detail = @event.Detail;
        var freelancer = await _freelancerRepository.GetByIdAsync(detail.Proposal.FreelancerId);

        var result = freelancer.SubtractCredits(detail.Credits);

        if (!result)
        {
            return new CreditsSubstractionResult(detail.AggregateId, detail.Proposal, Result.Fail);
        }

        await _freelancerRepository.SaveAsync(freelancer);
        return new CreditsSubstractionResult(detail.AggregateId, detail.Proposal, Result.Success);
    }
}
