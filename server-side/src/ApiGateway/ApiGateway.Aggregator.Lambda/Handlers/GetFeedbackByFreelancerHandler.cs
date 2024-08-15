using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using FeedbackManagement.Persistence;
using ReadModel;
using ReadModelStore;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace ApiGateway.Aggregator.Lambda.Handlers;

public class GetFeedbackByFreelancerHandler
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IJobReadModelRepository _jobRepository;

    private ILambdaContext _context;

    public GetFeedbackByFreelancerHandler()
    {
        _feedbackRepository = new FeedbackRepository();
        _jobRepository = new JobReadModelRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var freelancerId = Guid.Parse(request.PathParameters["freelancerId"]);

        try
        {
            var feedbacks = await _feedbackRepository.GetByFreelancer(freelancerId);
            var jobsTitle = (await _jobRepository.GetByIdsAsync(new HashSet<Guid>(feedbacks.Select(x => x.JobId)).ToHashSet())).ToDictionary(x => x.Id, x => x.Title);

            var feedbacksAggregated = new List<Models.Feedback>();
            foreach (var feedback in feedbacks)
            {
                feedbacksAggregated.Add(new Models.Feedback(feedback, jobsTitle.GetValueOrDefault(feedback.JobId)));
            }

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(feedbacksAggregated, JsonOptions.Options),
                Headers = Headers.CORS
            };
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"ERROR - {ex}\nSTACK TRACE - {ex.StackTrace}");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 500,
                Headers = Headers.CORS
            };
        }
    }
}
