using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ApiGateway.Aggregator.Lambda.Models;
using ClientProfile.Lambda.Persistence;
using Common.Layer.Headers;
using Common.Layer.JsonOptions;
using ReadModel;
using ReadModelStore;
using RealTime.Chat.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace ApiGateway.Aggregator.Lambda.Handlers;

public class GetChatsHandler
{
    private readonly IChatRepository _chatRepository;
    private readonly IJobReadModelRepository _jobRepository;
    private readonly IFreelancerReadModelRepository _freelancerRepository;
    private readonly IClientRepository _clientRepository;
    private ILambdaContext _context;

    public GetChatsHandler()
    {
        _chatRepository = new ChatRepository();
        _jobRepository = new JobReadModelRepository();
        _freelancerRepository = new FreelancerReadModelRepository();
        _clientRepository = new ClientRepository();
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _context = context;
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(request.Headers["Authorization"].Replace("Bearer ", ""));
        var sub = Guid.Parse(jwtToken.Subject);

        try
        {
            var chatsAggregated = new List<Chat>();
            var chats = await _chatRepository.GetByParticipantAsync(sub);
            foreach (var chat in chats)
            {
                var freelancer = await _freelancerRepository.GetByIdAsync(chat.FreelancerId);
                var client = await _clientRepository.GetByIdAsync(chat.ClientId);
                var job = await _jobRepository.GetByIdAsync(chat.JobId);
                chatsAggregated.Add(new Chat(chat, freelancer, client, job));
            }

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(chatsAggregated, JsonOptions.Options),
                Headers = Headers.CORS
            };
        }
        catch (Exception ex)
        {
            _context.Logger.LogError(ex.ToString());

            return new APIGatewayProxyResponse()
            {
                StatusCode = 500,
                Headers = Headers.CORS
            };
        }
    }

}
