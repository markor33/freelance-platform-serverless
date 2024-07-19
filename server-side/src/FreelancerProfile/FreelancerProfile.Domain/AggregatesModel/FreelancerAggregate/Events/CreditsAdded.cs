using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class CreditsAdded : DomainEvent
    {
        public int Amount { get; private set; }

        [JsonConstructor]
        public CreditsAdded(Guid aggregateId, int amount) : base(aggregateId)
        {
            Amount = amount;
        }
    }
}
