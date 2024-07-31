using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.JsonOptions;
using RealTime.Chat.Models;
using RealTime.Chat.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace RealTime.Chat.Handlers;

public class SendMessageCommandHandler
{
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IConnectionMappingRepository _connectionMappingRepository;
    private readonly IAmazonApiGatewayManagementApi _apiGatewayManagementApi;

    public SendMessageCommandHandler()
    {
        _chatRepository = new ChatRepository();
        _messageRepository = new MessageRepository();
        _connectionMappingRepository = new ConnectionMappingRepository();
        _apiGatewayManagementApi = new AmazonApiGatewayManagementApiClient();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].Replace("Bearer ", ""));
        var sub = Guid.Parse(jwtToken.Subject);

        try
        {
            var command = JsonSerializer.Deserialize<SendMessageCommand>(request.Body, JsonOptions.Options);
            var chat = await _chatRepository.GetByIdAsync(command.ChatId);

            var message = new Message(chat.Id, sub, command.Text);
            await _messageRepository.SaveAsync(message);

            var sendToId = (chat.ClientId == sub) ? chat.FreelancerId : chat.ClientId;

            var connectionMapping = await _connectionMappingRepository.GetAsync(sub);

            if (connectionMapping == null)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 404,
                    Body = "Connection not found"
                };
            }

            await SendMessageToConnectionAsync(connectionMapping.ConnectionId, command.Text);

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = "Message sent successfully"
            };

        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error: {ex.Message}");

            return new APIGatewayProxyResponse
            {
                StatusCode = 500,
                Body = "Internal Server Error"
            };
        }
    }

    private async Task SendMessageToConnectionAsync(string connectionId, string message)
    {
        try
        {
            var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            using var memoryStream = new MemoryStream(messageBytes);

            var request = new PostToConnectionRequest
            {
                ConnectionId = connectionId,
                Data = memoryStream
            };

            await _apiGatewayManagementApi.PostToConnectionAsync(request);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error sending message to connection {connectionId}: {ex.Message}");
        }
    }
}

public class SendMessageCommand
{
    public Guid ChatId { get; set; }
    public string Text { get; set; }
}