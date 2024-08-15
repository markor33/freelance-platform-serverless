using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using JobManagement.Domain.Repositories;
using JobManagement.Infrastructure.Repositories;
using Amazon;
using Amazon.Lambda.Core;
using System.Text.Json.Serialization;
using Common.Layer.EventBus;

namespace InitialMessageSent.Lambda.Handlers;

public class InitialMessageSentCommandHandler
{
    private readonly IJobRepository _jobRepository;

    public InitialMessageSentCommandHandler()
    {
        var client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        var context = new DynamoDBContext(client);
        _jobRepository = new JobRepository(context);
    }

    public async Task FunctionHandler(EventBusEvent<InitialMessageSentEvent> @event, ILambdaContext context)
    {
        var detail = @event.Detail;
        var job = await _jobRepository.GetByIdAsync(detail.JobId);

        job.SetProposalStatusToInterview(detail.ProposalId);

        await _jobRepository.SaveAsync(job);
    }
}

public class InitialMessageSentEvent
{
    public Guid JobId { get; set; }
    public Guid ProposalId { get; set; }
    public Guid FreelancerId { get; set; }

    public InitialMessageSentEvent() { }

    [JsonConstructor]
    public InitialMessageSentEvent(Guid jobId, Guid proposalId, Guid freelancerId)
    {
        JobId = jobId;
        ProposalId = proposalId;
        FreelancerId = freelancerId;
    }
}
