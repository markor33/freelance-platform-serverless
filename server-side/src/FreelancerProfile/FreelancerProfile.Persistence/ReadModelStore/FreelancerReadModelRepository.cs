using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using ReadModel;
using Amazon;

namespace ReadModelStore;

public class FreelancerReadModelRepository : IFreelancerReadModelRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public FreelancerReadModelRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<FreelancerViewModel> GetByIdAsync(Guid id)
    {
        return await _context.LoadAsync<FreelancerViewModel>(id);
    }

    public async Task<List<FreelancerViewModel>> GetByIds(List<Guid> ids)
    {
        var batchGet = _context.CreateBatchGet<FreelancerViewModel>();
        foreach (var id in ids)
        {
            batchGet.AddKey(id);
        }
        await batchGet.ExecuteAsync();
        return batchGet.Results;
    }

    public async Task<List<FreelancerViewModel>> GetByIdsAsync(HashSet<Guid> ids)
    {
        var batchGet = _context.CreateBatchGet<FreelancerViewModel>();

        foreach (var id in ids)
        {
            batchGet.AddKey(id);
        }

        await batchGet.ExecuteAsync();

        return batchGet.Results;
    }

    public async Task SaveAsync(FreelancerViewModel freelancer)
    {
        await _context.SaveAsync<FreelancerViewModel>(freelancer);
    }

}
