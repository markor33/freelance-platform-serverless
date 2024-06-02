using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate;
using FreelancerProfile.Domain.Repositories;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json;

namespace WriteModel;

public class FreelancerRepository : IFreelancerRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public FreelancerRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<Freelancer> GetByIdAsync(Guid id)
    {
        var domainEventLogs = (await _context.QueryAsync<DomainEventLog>(id.ToString()).GetRemainingAsync()).OrderBy(eventLog => eventLog.Created);

        var freelancer = new Freelancer();

        foreach (var domainEventLog in domainEventLogs)
        {
            var eventType = Type.GetType(domainEventLog.EventType);
            var domainEvent = (DomainEvent)JsonSerializer.Deserialize(domainEventLog.EventData, eventType);
            freelancer.Apply(domainEvent);
        }

        return freelancer;
    }

    public async Task SaveAsync(Freelancer freelancer)
    {
        var domainEvents = freelancer.Changes.Select(@event => new DomainEventLog(@event));

        var domainEventBatch = _context.CreateBatchWrite<DomainEventLog>();
        domainEventBatch.AddPutItems(domainEvents);

        await domainEventBatch.ExecuteAsync();
    }
}

