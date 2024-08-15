using JobManagement.Domain.SeedWork;
using JobManagement.Domain.AggregatesModel.JobAggregate;
using JobManagement.Domain.Repositories;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using System.Reflection;
using JobManagement.Infrastructure.EventStore;
using System.Text.Json;
using Amazon;

namespace JobManagement.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly IDynamoDBContext _context;

        public JobRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<Job> GetByIdAsync(Guid id)
        {
            var domainEventLogs = (await _context.QueryAsync<DomainEventLog>(id.ToString()).GetRemainingAsync()).OrderBy(eventLog => eventLog.Created);

            var job = new Job();

            var assemblyWithEvents = Assembly.GetAssembly(typeof(Job));

            foreach (var domainEventLog in domainEventLogs)
            {
                var eventType = assemblyWithEvents.GetType($"JobManagement.Domain.AggregatesModel.JobAggregate.Events.{domainEventLog.EventType}");
                var domainEvent = (DomainEvent)JsonSerializer.Deserialize(domainEventLog.EventData, eventType);
                job.Apply(domainEvent);
            }

            return job;
        }

        public async Task<Job> SaveAsync(Job job)
        {
            var domainEvents = job.Changes.Select(@event => new DomainEventLog(@event));

            var domainEventBatch = _context.CreateBatchWrite<DomainEventLog>();
            domainEventBatch.AddPutItems(domainEvents);

            await domainEventBatch.ExecuteAsync();

            return job;
        }
    }
}
