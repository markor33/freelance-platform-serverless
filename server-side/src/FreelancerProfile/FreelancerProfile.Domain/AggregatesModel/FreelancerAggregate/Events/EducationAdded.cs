using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class EducationAdded : DomainEvent
    {
        public Education Education { get; private set; }

        [JsonConstructor]
        public EducationAdded(Guid aggregateId, Education education) : base(aggregateId)
        {
            Education = education;
        }
    }
}
