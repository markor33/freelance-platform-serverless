using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobSearch.Lambda.IntegrationEvents;
using JobSearch.Persistence;

namespace JobSearch.Lambda.Handlers;

public class JobDeletedHandler
{
    private readonly IJobSearchRepository _repository;

    public JobDeletedHandler()
    {
        _repository = new JobSearchRepository();
    }

    public async Task FunctionHandler(EventBusEvent<JobDeleted> @event, ILambdaContext context)
    {
        try
        {
            var detail = @event.Detail;
            var job = await _repository.GetById(detail.AggregateId);
            job.Status = IndexModel.JobStatus.REMOVED;
            await _repository.Update(job);
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"ERROR MSG - {ex}\nSTACK TRACE - {ex.StackTrace}");
        }
    }
}
