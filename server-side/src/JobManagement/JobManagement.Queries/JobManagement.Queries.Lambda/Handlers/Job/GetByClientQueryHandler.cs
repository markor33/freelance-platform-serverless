using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using ReadModel;
using ReadModelStore;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace JobManagement.Queries.Lambda.Handlers.Job;

public class GetByClientQueryHandler
{
    private readonly IJobReadModelRepository _repository;

    public GetByClientQueryHandler()
    {
        _repository = new JobReadModelRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].Replace("Bearer ", ""));
        var sub = jwtToken.Subject;

        var jobs = await _repository.GetByClient(Guid.Parse(sub));

        return new APIGatewayProxyResponse()
        {
            StatusCode = 200,
            Body = JsonSerializer.Serialize(jobs, JsonOptions.Options),
            Headers = Headers.CORS
        };
    }
}
