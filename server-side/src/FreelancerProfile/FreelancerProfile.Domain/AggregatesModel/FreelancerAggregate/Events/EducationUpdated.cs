using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class EducationUpdated : DomainEvent
    {
        public Guid EducationId { get; private set; }
        public string SchoolName { get; private set; }
        public string Degree { get; private set; }
        public DateRange Attended { get; private set; }

        [JsonConstructor]
        public EducationUpdated(Guid aggregateId, Guid educationId, string schoolName, string degree, DateRange attended) : base(aggregateId)
        {
            EducationId = educationId;
            SchoolName = schoolName;
            Degree = degree;
            Attended = attended;
        }

    }
}
