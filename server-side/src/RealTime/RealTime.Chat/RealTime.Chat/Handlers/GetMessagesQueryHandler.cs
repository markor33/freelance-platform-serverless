using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using RealTime.Chat.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace RealTime.Chat.Handlers;

public class GetMessagesQueryHandler
{
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;

    public GetMessagesQueryHandler()
    {
        _chatRepository = new ChatRepository();
        _messageRepository = new MessageRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].Replace("Bearer ", ""));
        var sub = Guid.Parse(jwtToken.Subject);

        var id = Guid.Parse(request.PathParameters["id"]);

        try
        {
            var chat = await _chatRepository.GetByIdAsync(id);
            if (chat.ClientId != sub && chat.FreelancerId != sub)
            {
                return new APIGatewayProxyResponse()
                {
                    StatusCode = 404,
                    Headers = Headers.CORS
                };
            }

            var messages = await _messageRepository.GetByChat(id);

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(messages, JsonOptions.Options),
                Headers = Headers.CORS
            };
        }
        catch
        {
            return new APIGatewayProxyResponse()
            {
                StatusCode = 400,
                Headers = Headers.CORS
            };
        }
    }

}
