using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using ReadModel;
using ReadModelStore;
using System.Text.Json;

namespace JobManagement.Queries.Lambda.Handlers.Job;

public class GetAllQueryHandler
{
    private readonly IJobReadModelRepository _repository;

    public GetAllQueryHandler()
    {
        _repository = new JobReadModelRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var jobs = await _repository.GetAllAsync();

        return new APIGatewayProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(jobs, JsonOptions.Options),
            Headers = Headers.CORS
        };
    }
}
