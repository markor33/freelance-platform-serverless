using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using JobManagement.ReadModel;
using ReadModel;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;

namespace JobManagement.ReadModelStore;

public class ContractReadModelRepository : IContractReadModelRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public ContractReadModelRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<ContractViewModel> GetById(Guid id, Guid jobId)
    {
        return await _context.LoadAsync<ContractViewModel>(id, jobId);
    }

    public async Task<List<ContractViewModel>> GetByJob(Guid jobId)
    {
        var conditions = new List<ScanCondition>
        {
            new("JobId", ScanOperator.Equal, jobId)
        };

        var contracts = await _context.ScanAsync<ContractViewModel>(conditions).GetRemainingAsync();

        return contracts;
    }

    public async Task<List<ContractViewModel>> GetByClient(Guid clientId)
    {
        var conditions = new List<ScanCondition>
        {
            new("ClientId", ScanOperator.Equal, clientId)
        };

        var contracts = await _context.ScanAsync<ContractViewModel>(conditions).GetRemainingAsync();

        return contracts;
    }

    public async Task SaveAsync(ContractViewModel contract)
    {
        await _context.SaveAsync<ContractViewModel>(contract);
    }

    public async Task<List<ContractViewModel>> GetByFreelancer(Guid freelancerId)
    {
        var conditions = new List<ScanCondition>
        {
            new("FreelancerId", ScanOperator.Equal, freelancerId)
        };

        var contracts = await _context.ScanAsync<ContractViewModel>(conditions).GetRemainingAsync();

        return contracts;
    }
}
