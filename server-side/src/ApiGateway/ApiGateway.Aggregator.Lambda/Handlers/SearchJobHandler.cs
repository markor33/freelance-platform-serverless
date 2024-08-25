using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ApiGateway.Aggregator.Lambda.Models;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using FeedbackManagement.Persistence;
using JobSearch.Persistence;
using System.Text.Json;

namespace ApiGateway.Aggregator.Lambda.Handlers;

public class SearchJobHandler
{
    private readonly IJobSearchRepository _jobSearchRepository;
    private readonly IFeedbackRepository _feedbackRepository;

    public SearchJobHandler()
    {
        _jobSearchRepository = new JobSearchRepository();
        _feedbackRepository = new FeedbackRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var jobSearchParams = JsonSerializer.Deserialize<JobSearchParams>(request.Body, JsonOptions.Options);

        try
        {
            var jobsAggregated = new List<Job>();
            var jobs = await _jobSearchRepository.Search(jobSearchParams);

            if (jobs.Count > 0)
            {
                var clientIds = jobs.Select(x => x.ClientId).ToHashSet();
                var clientFeedbacks = await _feedbackRepository.GetAverageRatingByClients(clientIds);
                context.Logger.LogInformation(string.Join(", ", clientIds.Select(x => $"{x}").ToList()));
                foreach (var job in jobs)
                {
                    jobsAggregated.Add(new Job(job, clientFeedbacks.GetValueOrDefault(job.ClientId)));
                }
            }

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(jobsAggregated, JsonOptions.Options),
                Headers = Headers.CORS
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"ERROR - {ex}\nSTACK TRACE - {ex.StackTrace}");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 500,
                Headers = Headers.CORS
            };
        }
    }

}