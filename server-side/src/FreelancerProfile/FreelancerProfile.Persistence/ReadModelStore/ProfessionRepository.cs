using Amazon;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.Repositories;
using ReadModel;

namespace ReadModelStore;

public class ProfessionRepository : IProfessionRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public ProfessionRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<Profession> GetByIdAsync(Guid id)
    {
        var professionViewModel = await _context.LoadAsync<ProfessionViewModel>(id);
        
        return professionViewModel.ToProfession();
    }
}
