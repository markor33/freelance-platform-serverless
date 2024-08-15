using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using RealTime.Chat.Persistence;
using System.IdentityModel.Tokens.Jwt;

namespace RealTime.Chat.Handlers;

public class DisconnectHandler
{
    private readonly IConnectionMappingRepository _connectionMappingRepository;

    public DisconnectHandler()
    {
        _connectionMappingRepository = new ConnectionMappingRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var connectionId = request.RequestContext.ConnectionId;

        try
        {
            await _connectionMappingRepository.DeleteByConnectionAsync(connectionId);

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
