using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.IdentityModel.Tokens.Jwt;
using RealTime.Chat.Persistence;
using RealTime.Chat.Models;
using Common.Layer.Headers;

namespace RealTime.Chat.Handlers;

public class ConnectHandler
{
    private readonly IConnectionMappingRepository _connectionMappingRepository;

    public ConnectHandler()
    {
        _connectionMappingRepository = new ConnectionMappingRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.QueryStringParameters["Authorization"]);
        var sub = Guid.Parse(jwtToken.Subject);
        var connectionId = request.RequestContext.ConnectionId;

        try
        {
            var connectionMapping = new ConnectionMapping()
            {
                Sub = sub,
                ConnectionId = connectionId,
            };

            await _connectionMappingRepository.SaveAsync(connectionMapping);

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Headers = Headers.CORS
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error: {ex.Message}");

            return new APIGatewayProxyResponse()
            {
                StatusCode = 404,
                Headers = Headers.CORS
            };
        }
    }
}
