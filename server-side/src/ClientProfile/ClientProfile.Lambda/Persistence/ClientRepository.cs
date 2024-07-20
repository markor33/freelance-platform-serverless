using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon;

namespace ClientProfile.Lambda.Persistence;

public interface IClientRepository
{
    Task<Client> GetByIdAsync(Guid id);
    Task SaveAsync(Client client);
    Task DeleteAsync(Guid id);
}

public class ClientRepository : IClientRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public ClientRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<Client> GetByIdAsync(Guid id)
    {
        return await _context.LoadAsync<Client>(id);
    }

    public async Task SaveAsync(Client client)
    {
        var batch = _context.CreateBatchWrite<Client>();
        batch.AddPutItem(client);

        await batch.ExecuteAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var client = await GetByIdAsync(id);
        await _context.DeleteAsync(client);
    }

}
