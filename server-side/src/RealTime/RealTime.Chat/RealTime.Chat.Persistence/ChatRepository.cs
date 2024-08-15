using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace RealTime.Chat.Persistence;

public interface IChatRepository
{
    Task<Models.Chat> GetByIdAsync(Guid id);
    Task<List<Models.Chat>> GetByParticipantAsync(Guid participantId);
    Task SaveAsync(Models.Chat chat);
}

public class ChatRepository : IChatRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public ChatRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<Models.Chat> GetByIdAsync(Guid id)
    {
        return await _context.LoadAsync<Models.Chat>(id);
    }

    public async Task<List<Models.Chat>> GetByParticipantAsync(Guid participantId)
    {
        var conditions = new List<ScanCondition>
        {
            new("ClientId", ScanOperator.Equal, participantId)
        };

        var request = new ScanRequest
        {
            TableName = "Chats",
            FilterExpression = "ClientId = :participantId OR FreelancerId = :participantId",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":participantId", new AttributeValue { S = participantId.ToString() } }
            }
        };

        var response = await _client.ScanAsync(request);

        var chats = response.Items.Select(item => new Models.Chat
        {
            Id = Guid.Parse(item["Id"].S),
            ClientId = Guid.Parse(item["ClientId"].S),
            FreelancerId = Guid.Parse(item["FreelancerId"].S),
            JobId = Guid.Parse(item["JobId"].S),
            ProposalId = Guid.Parse(item["ProposalId"].S),
        }).ToList();

        return chats;
    }

    public async Task SaveAsync(Models.Chat chat)
    {
        await _context.SaveAsync<Models.Chat>(chat);
    }
}
