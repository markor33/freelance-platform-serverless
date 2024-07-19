using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class CreditsSubtracted : DomainEvent
    {
        public int Amount { get; private set; }

        [JsonConstructor]
        public CreditsSubtracted(Guid aggregateId, int amount) : base(aggregateId)
        {
            Amount = amount;
        }
    }
}
