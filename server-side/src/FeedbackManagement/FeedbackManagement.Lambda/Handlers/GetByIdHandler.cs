using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using FeedbackManagement.Persistence;
using System.Text.Json;

namespace FeedbackManagement.Lambda.Handlers;

public class GetByIdHandler
{
    private readonly IFeedbackRepository _repository;

    public GetByIdHandler()
    {
        _repository = new FeedbackRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var id = Guid.Parse(request.PathParameters["id"]);

        var finishedContract = await _repository.GetById(id);

        return new APIGatewayProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(finishedContract, JsonOptions.Options),
            Headers = Headers.CORS
        };
    }

}
