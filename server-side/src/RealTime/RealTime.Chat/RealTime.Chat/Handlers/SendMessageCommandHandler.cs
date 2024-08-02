using Amazon;
using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Common.Layer.JsonOptions;
using RealTime.Chat.Models;
using RealTime.Chat.Persistence;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RealTime.Chat.Handlers;

public class SendMessageCommandHandler
{
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IConnectionMappingRepository _connectionMappingRepository;
    private readonly IAmazonApiGatewayManagementApi _apiGatewayManagementApi;
    private ILambdaContext _context;

    public SendMessageCommandHandler()
    {
        _chatRepository = new ChatRepository();
        _messageRepository = new MessageRepository();
        _connectionMappingRepository = new ConnectionMappingRepository();
        _apiGatewayManagementApi = new AmazonApiGatewayManagementApiClient(new AmazonApiGatewayManagementApiConfig()
        {
            ServiceURL = Environment.GetEnvironmentVariable("SERVICE_URL")
        });
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var connectionId = request.RequestContext.ConnectionId;

        try
        {
            var command = JsonSerializer.Deserialize<SendMessageCommand>(request.Body, JsonOptions.Options);
            var chat = await _chatRepository.GetByIdAsync(command.ChatId);

            var senderConnectionMapping = await _connectionMappingRepository.GetByConnectionAsync(connectionId);

            var message = new Message(chat.Id, senderConnectionMapping.Sub, command.Text);
            await _messageRepository.SaveAsync(message);

            var sendToId = (chat.ClientId == senderConnectionMapping.Sub) ? chat.FreelancerId : chat.ClientId;

            var connectionMapping = await _connectionMappingRepository.GetAsync(sendToId);

            if (connectionMapping == null)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 404,
                    Body = "Connection not found"
                };
            }

            await SendMessageToConnectionAsync(connectionMapping.ConnectionId, command.Text);

            _context.Logger.LogInformation("GOTOVO");

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = "Message sent successfully"
            };

        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Error: {ex}");

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

            _context.Logger.LogInformation($"CID {connectionId}");

            var request = new PostToConnectionRequest
            {
                ConnectionId = connectionId,
                Data = new MemoryStream()
            };

            await _apiGatewayManagementApi.PostToConnectionAsync(request);
        }
        catch (Exception ex)
        {
            _context.Logger.LogError($"ERROR: {ex}");
            throw new Exception($"Error sending message to connection {connectionId}: {ex.Message}");
        }
    }
}

public class SendMessageCommand
{
    public Guid ChatId { get; set; }
    public string Text { get; set; }

}