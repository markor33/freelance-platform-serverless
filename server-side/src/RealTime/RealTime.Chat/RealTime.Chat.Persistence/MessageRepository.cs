using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using RealTime.Chat.Models;
using Amazon;

namespace RealTime.Chat.Persistence;

public interface IMessageRepository
{
    Task<List<Message>> GetByChat(Guid chatId);
    Task SaveAsync(Message message);
}

public class MessageRepository : IMessageRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public MessageRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<List<Message>> GetByChat(Guid chatId)
    {
        return await _context.QueryAsync<Message>(chatId).GetRemainingAsync();
    }

    public async Task SaveAsync(Message message)
    {
        await _context.SaveAsync(message);
    }
}
