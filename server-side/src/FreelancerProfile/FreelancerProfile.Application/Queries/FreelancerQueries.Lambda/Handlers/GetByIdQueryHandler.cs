using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using ReadModel;
using ReadModelStore;
using System.Text.Json;

namespace FreelancerQueries.Lambda.Handlers;

public class GetByIdQueryHandler
{
    private readonly IFreelancerReadModelRepository _repository;

    public GetByIdQueryHandler()
    {
        _repository = new FreelancerReadModelRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var id = Guid.Parse(request.PathParameters["id"]);

        var freelancer = await _repository.GetByIdAsync(id);
        
        if (freelancer == null)
            return new APIGatewayProxyResponse()
            {
                StatusCode = 404
            };

        return new APIGatewayProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(freelancer, JsonOptions.Options),
            Headers = Headers.CORS
        };
    }
}
