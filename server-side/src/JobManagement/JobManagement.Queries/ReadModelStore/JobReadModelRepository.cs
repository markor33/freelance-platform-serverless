using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using ReadModel;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;

namespace ReadModelStore;

public class JobReadModelRepository : IJobReadModelRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public JobReadModelRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<List<JobViewModel>> GetAllAsync()
    {
        var scanConditions = new List<ScanCondition>();
        var allJobs = await _context.ScanAsync<JobViewModel>(scanConditions).GetRemainingAsync();
        return allJobs;
    }

    public async Task<List<JobViewModel>> GetByIdsAsync(HashSet<Guid> ids)
    {
        var batchGet = _context.CreateBatchGet<JobViewModel>();
        foreach (var id in ids)
        {
            batchGet.AddKey(id);
        }
        await batchGet.ExecuteAsync();
        return batchGet.Results;
    }

    public async Task<List<JobViewModel>> GetByClient(Guid clientId)
    {
        var queryConditions = new List<ScanCondition>
            {
                new("ClientId", ScanOperator.Equal, clientId),
                new("Status", ScanOperator.NotEqual, JobStatus.REMOVED)
            };
        var clientJobs = await _context.ScanAsync<JobViewModel>(queryConditions).GetRemainingAsync();
        return clientJobs;
    }

    public async Task<JobViewModel> GetByIdAsync(Guid id)
    {
        return await _context.LoadAsync<JobViewModel>(id);
    }

    public async Task SaveAsync(JobViewModel jobViewModel)
    {
        await _context.SaveAsync<JobViewModel>(jobViewModel);
    }
}
