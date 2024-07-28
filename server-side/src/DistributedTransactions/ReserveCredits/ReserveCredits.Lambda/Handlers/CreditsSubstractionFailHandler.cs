using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using JobManagement.Domain.Repositories;
using JobManagement.Infrastructure.Repositories;
using Amazon;

namespace ReserveCredits.Lambda.Handlers;

public class CreditsSubstractionFailHandler
{
    private readonly IJobRepository _jobRepository;

    public CreditsSubstractionFailHandler()
    {
        var client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        var context = new DynamoDBContext(client);
        _jobRepository = new JobRepository(context);
    }

    public async Task FunctionHandler(CreditsSubstractionResult result, ILambdaContext context)
    {
        var job = await _jobRepository.GetByIdAsync(result.JobId);

        job.RemoveProposal(result.Proposal.Id);

        await _jobRepository.SaveAsync(job);
    }
}
