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

    public async Task SaveAsync(FreelancerViewModel freelancer)
    {
        await _context.SaveAsync<FreelancerViewModel>(freelancer);
    }

}
