using Amazon.Lambda.Core;
using Common.Layer.EventBus;
using JobSearch.IndexModel;
using JobSearch.Lambda.IntegrationEvents;
using JobSearch.Persistence;

namespace JobSearch.Lambda.Handlers;

public class JobUpdatedHandler
{
    private readonly IJobSearchRepository _repository;

    public JobUpdatedHandler()
    {
        _repository = new JobSearchRepository();
    }

    public async Task FunctionHandler(EventBusEvent<JobUpdated> @event, ILambdaContext context)
    {
        try
        {
            var detail = @event.Detail;

            var job = new Job()
            {
                Id = detail.AggregateId,
                ClientId = detail.ClientId,
                ProfessionId = detail.ProfessionId,
                Title = detail.Title,
                Description = detail.Description,
                Created = detail.Created,
                Status = detail.Status,
                Payment = detail.Payment,
                Credits = detail.Credits,
                ExperienceLevel = detail.ExperienceLevel,
                NumOfProposals = 0,
                NumOfCurrInterviews = 0,
            };

            await _repository.Update(job);
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"ERROR MSG - {ex}\nSTACK TRACE - {ex.StackTrace}");
        }
    }
}
