using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;
using System.Text.Json;

namespace JobManagement.Queries.Lambda.Handlers.Proposal;

public class GetByIdQueryHandler
{
    private IProposalReadModelRepository _repository;

    public GetByIdQueryHandler()
    {
        _repository = new ProposalReadModelRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var id = Guid.Parse(request.PathParameters["proposalId"]);

        var proposal = await _repository.GetById(id);

        return new APIGatewayProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(proposal, JsonOptions.Options),
            Headers = Headers.CORS
        };
    }
}
