using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ApiGateway.Aggregator.Lambda.Models;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using FeedbackManagement.Persistence;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;
using ReadModel;
using ReadModelStore;
using System.Text.Json;

namespace ApiGateway.Aggregator.Lambda.Handlers;

public class GetJobProposalsHandler
{
    private readonly IProposalReadModelRepository _proposalRepository;
    private readonly IFreelancerReadModelRepository _freelancerRepository;
    private readonly IFeedbackRepository _feedbackRepository;

    public GetJobProposalsHandler()
    {
        _proposalRepository = new ProposalReadModelRepository();
        _freelancerRepository = new FreelancerReadModelRepository();
        _feedbackRepository = new FeedbackRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var id = Guid.Parse(request.PathParameters["id"]);

        var proposals = await _proposalRepository.GetByJob(id);

        if (proposals.Count == 0)
        {
            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(new List<Proposal>(), JsonOptions.Options),
                Headers = Headers.CORS
            };
        }

        var freelancerIds = proposals.Select(x => x.FreelancerId).ToHashSet();
        var freelancers = (await _freelancerRepository.GetByIdsAsync(freelancerIds)).ToDictionary(x => x.Id, x => x);
        var feedbacks = await _feedbackRepository.GetAverageRatingByFreelancers(freelancerIds);

        var aggregatedProposals = new List<Proposal>();
        foreach (var proposal in proposals)
        {
            var aggregatedProposal = new Proposal(proposal, freelancers.GetValueOrDefault(proposal.FreelancerId), feedbacks.GetValueOrDefault(proposal.FreelancerId));
            aggregatedProposals.Add(aggregatedProposal);
        }

        return new APIGatewayProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(aggregatedProposals, JsonOptions.Options),
            Headers = Headers.CORS
        };
    }
}
