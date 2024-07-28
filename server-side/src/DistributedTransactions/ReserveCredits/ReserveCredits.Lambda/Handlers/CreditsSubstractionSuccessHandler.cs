using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using JobManagement.Domain.Repositories;
using JobManagement.Infrastructure.Repositories;
using Amazon;
using Amazon.Lambda.Core;

namespace ReserveCredits.Lambda.Handlers;

public class CreditsSubstractionSuccessHandler
{
    private readonly IJobRepository _jobRepository;

    public CreditsSubstractionSuccessHandler()
    {
        var client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        var context = new DynamoDBContext(client);
        _jobRepository = new JobRepository(context);
    }

    public async Task FunctionHandler(CreditsSubstractionResult result, ILambdaContext context)
    {
        var job = await _jobRepository.GetByIdAsync(result.JobId);

        job.SetProposalStatusToSent(result.Proposal.Id);

        await _jobRepository.SaveAsync(job);
    }

}
