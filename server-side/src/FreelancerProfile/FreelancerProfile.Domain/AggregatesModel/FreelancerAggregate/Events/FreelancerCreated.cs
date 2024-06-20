using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{

    public class FreelancerCreated : DomainEvent
    {
        public Guid UserId { get; set; }

        public FreelancerCreated() : base() { }

        [JsonConstructor]
        public FreelancerCreated(Guid aggregateId, Guid userId) : base(aggregateId)
        {
            UserId = userId;
        }

    }
}
