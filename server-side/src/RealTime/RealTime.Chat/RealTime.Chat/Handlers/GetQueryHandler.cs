using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using RealTime.Chat.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace RealTime.Chat.Handlers;

public class GetQueryHandler
{
    private readonly IChatRepository _chatRepository;

    public GetQueryHandler()
    {
        _chatRepository = new ChatRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].Replace("Bearer ", ""));
        var sub = jwtToken.Subject;
        try
        {
            var chats = await _chatRepository.GetByParticipantAsync(Guid.Parse(sub));

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(chats, JsonOptions.Options),
                Headers = Headers.CORS
            };
        }
        catch (Exception ex)
        {
            return new APIGatewayProxyResponse()
            {
                StatusCode = 400,
                Headers = Headers.CORS
            };
        }
    }

}
