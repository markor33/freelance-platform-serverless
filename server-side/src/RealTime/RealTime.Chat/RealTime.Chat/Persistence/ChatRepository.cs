using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;

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
            new("ClientId", ScanOperator.Equal, participantId),
            new("FreelancerId", ScanOperator.Equal, participantId)
        };

        return await _context.ScanAsync<Models.Chat>(conditions).GetRemainingAsync();
    }

    public async Task SaveAsync(Models.Chat chat)
    {
        await _context.SaveAsync<Models.Chat>(chat);
    }
}
