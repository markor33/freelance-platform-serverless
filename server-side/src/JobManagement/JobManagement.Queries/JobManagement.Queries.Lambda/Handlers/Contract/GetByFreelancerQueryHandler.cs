using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using JobManagement.ReadModel;
using JobManagement.ReadModelStore;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace JobManagement.Queries.Lambda.Handlers.Contract;

public class GetByFreelancerQueryHandler
{
    private readonly IContractReadModelRepository _contractRepository;

    public GetByFreelancerQueryHandler()
    {
        _contractRepository = new ContractReadModelRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].Replace("Bearer ", ""));
        var sub = Guid.Parse(jwtToken.Subject);

        try
        {
            var contracts = await _contractRepository.GetByFreelancer(sub);

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(contracts, JsonOptions.Options),
                Headers = Headers.CORS
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError(ex.ToString());

            return new APIGatewayProxyResponse()
            {
                StatusCode = 500,
                Headers = Headers.CORS
            };
        }
    }

}
