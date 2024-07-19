using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class EmploymentDeleted : DomainEvent
    {
        public Guid EmploymentId { get; private set; }

        [JsonConstructor]
        public EmploymentDeleted(Guid aggregateId, Guid employmentId) : base(aggregateId)
        {
            EmploymentId = employmentId;
        }

    }
}
