using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    [method: JsonConstructor]
    public class FreelancerCreated(Guid aggregateId, Guid userId) : DomainEvent(aggregateId)
    {
        public Guid UserId { get; private set; } = userId;

    }
}
