using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobSearch.IndexModel;
using JobSearch.Lambda.IntegrationEvents;
using JobSearch.Persistence;

namespace JobSearch.Lambda.Handlers;

public class ProposalStatusChangedHandler
{
    private readonly IJobSearchRepository _repository;

    public ProposalStatusChangedHandler()
    {
        _repository = new JobSearchRepository();
    }

    public async Task FunctionHandler(EventBusEvent<ProposalStatusChanged> @event, ILambdaContext context)
    {
        try
        {
            var detail = @event.Detail;
            var job = await _repository.GetById(detail.AggregateId);

            if (detail.Status == ProposalStatus.SENT)
            {
                job.NumOfProposals += 1;
            }
            else if (detail.Status == ProposalStatus.INTERVIEW)
            {
                job.NumOfCurrInterviews += 1;
            }

            await _repository.Update(job);
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"ERROR MSG - {ex}\nSTACK TRACE - {ex.StackTrace}");
        }
    }
}
