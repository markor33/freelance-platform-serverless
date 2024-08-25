using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobSearch.Lambda.IntegrationEvents;
using JobSearch.Persistence;

namespace JobSearch.Lambda.Handlers;

public class JobDoneHandler
{
    private readonly IJobSearchRepository _repository;

    public JobDoneHandler()
    {
        _repository = new JobSearchRepository();
    }

    public async Task FunctionHandler(EventBusEvent<JobDone> @event, ILambdaContext context)
    {
        try
        {
            var detail = @event.Detail;
            var job = await _repository.GetById(detail.AggregateId);
            job.Status = IndexModel.JobStatus.DONE;
            await _repository.Update(job);
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"ERROR MSG - {ex}\nSTACK TRACE - {ex.StackTrace}");
        }
    }

}
