using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using JobManagement.ReadModel;
using ReadModel;
using Amazon;
using Amazon.DynamoDBv2.DocumentModel;

namespace JobManagement.ReadModelStore;

public class ProposalReadModelRepository : IProposalReadModelRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public ProposalReadModelRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<ProposalViewModel> GetById(Guid id)
    {
        return await _context.LoadAsync<ProposalViewModel>(id);
    }

    public async Task<List<ProposalViewModel>> GetByJob(Guid jobId)
    {
        var conditions = new List<ScanCondition>
        {
            new("JobId", ScanOperator.Equal, jobId)
        };

        var proposals = await _context.ScanAsync<ProposalViewModel>(conditions).GetRemainingAsync();

        return proposals;
    }

    public async Task SaveAsync(ProposalViewModel proposal)
    {
        await _context.SaveAsync<ProposalViewModel>(proposal);
    }

    public async Task DeleteAsync(Guid proposalId)
    {
        await _context.DeleteAsync<ProposalViewModel>(proposalId);
    }
}
