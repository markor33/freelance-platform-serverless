using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class EducationDeleted : DomainEvent
    {
        public Guid EducationId { get; private set; }

        [JsonConstructor]
        public EducationDeleted(Guid aggregateId, Guid educationId) : base(aggregateId)
        {
            EducationId = educationId;
        }

    }
}
