using EventBus;
using Amazon.EventBridge;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using RealTime.Chat.Models;
using RealTime.Chat.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace RealTime.Chat.Handlers;

public class CreateChatCommandHandler
{
    private readonly IChatRepository _chatRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IAmazonEventBridge _eventBridgeClient = new AmazonEventBridgeClient();
    private ILambdaContext _context;

    public CreateChatCommandHandler()
    {
        _chatRepository = new ChatRepository();
        _messageRepository = new MessageRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].Replace("Bearer ", ""));
        var sub = jwtToken.Subject;

        var command = JsonSerializer.Deserialize<CreateChatCommand>(request.Body, JsonOptions.Options);
        command.ClientId = Guid.Parse(sub);

        var result = await CommandHandler(command);

        var statusCode = (result) ? 200 : 404;

        return new APIGatewayProxyResponse()
        {
            StatusCode = statusCode,
            Headers = Headers.CORS
        };
    }

    public async Task<bool> CommandHandler(CreateChatCommand request)
    {
        try
        {
            var chat = new Models.Chat(request.ClientId, request.FreelancerId, request.JobId, request.ProposalId);
            await _chatRepository.SaveAsync(chat);

            var message = new Message(chat.Id, request.ClientId, request.InitialMessage);
            await _messageRepository.SaveAsync(message);

            await _eventBridgeClient.PublishEvent(new InitialMessageSentEvent(request.JobId, request.ProposalId, request.FreelancerId));

            return true;
        }
        catch (Exception ex)
        {
            _context.Logger.LogError(ex.ToString());
            return false;
        }
    }

}

public class CreateChatCommand
{
    public Guid ClientId { get; set; }
    public Guid JobId { get; set; }
    public Guid ProposalId { get; set; }
    public Guid FreelancerId { get; set; }
    public string InitialMessage { get; set; } = string.Empty;
}

public class InitialMessageSentEvent
{
    public Guid JobId { get; private set; }
    public Guid ProposalId { get; private set; }
    public Guid FreelancerId { get; private set; }

    public InitialMessageSentEvent(Guid jobId, Guid proposalId, Guid freelancerId)
    {
        JobId = jobId;
        ProposalId = proposalId;
        FreelancerId = freelancerId;
    }
}