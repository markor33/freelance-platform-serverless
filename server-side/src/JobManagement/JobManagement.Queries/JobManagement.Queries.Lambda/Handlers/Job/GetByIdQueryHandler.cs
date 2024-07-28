using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using ReadModel;
using ReadModelStore;
using System.Text.Json;

namespace JobManagement.Queries.Lambda.Handlers.Job;

public class GetByIdQueryHandler
{
    private readonly IJobReadModelRepository _repository;

    public GetByIdQueryHandler()
    {
        _repository = new JobReadModelRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var id = Guid.Parse(request.PathParameters["id"]);

        var job = await _repository.GetByIdAsync(id);

        return new APIGatewayProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(job, JsonOptions.Options),
            Headers = Headers.CORS
        };
    }
}
