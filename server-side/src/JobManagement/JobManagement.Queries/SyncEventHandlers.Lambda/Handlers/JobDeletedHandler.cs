using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.Events;
using ReadModel;
using ReadModelStore;

namespace SyncEventHandlers.Lambda.Handlers;

public class JobDeletedHandler
{
    private readonly IJobReadModelRepository _repository;

    public JobDeletedHandler()
    {
        _repository = new JobReadModelRepository();
    }

    public async Task FunctionHandler(EventBusEvent<JobDeleted> @event, ILambdaContext context)
    {
        try
        {
            var detail = @event.Detail;
            var job = await _repository.GetByIdAsync(detail.AggregateId);
            job.Status = JobStatus.REMOVED;
            await _repository.SaveAsync(job);
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"ERROR MSG - {ex}\nSTACK TRACE - {ex.StackTrace}");
        }
    }
}
