using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using RealTime.Chat.Models;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;

namespace RealTime.Chat.Persistence;

public interface IConnectionMappingRepository
{
    Task<ConnectionMapping> GetAsync(Guid sub);
    Task SaveAsync(ConnectionMapping connectionMapping);
    Task DeleteAsync(Guid sub);
    Task DeleteByConnectionAsync(string connectionId);
}

public class ConnectionMappingRepository : IConnectionMappingRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public ConnectionMappingRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<ConnectionMapping> GetAsync(Guid sub)
    {
        return await _context.LoadAsync<ConnectionMapping>(sub);
    }

    public async Task SaveAsync(ConnectionMapping connectionMapping)
    {
        await _context.SaveAsync(connectionMapping);
    }

    public async Task DeleteAsync(Guid sub)
    {
        var connectionMapping = GetAsync(sub);
        await _context.DeleteAsync(connectionMapping);
    }

    public async Task DeleteByConnectionAsync(string connectionId)
    {
        var conditions = new List<ScanCondition>
            {
                new("ConnectionId", ScanOperator.Equal, connectionId)
            };

        var search = _context.ScanAsync<ConnectionMapping>(conditions);
        var results = await search.GetNextSetAsync();

        await _context.DeleteAsync(results[0]);
    }
}
