using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class CertificationUpdated : DomainEvent
    {
        public Guid CertificationId { get; private set; }
        public string Name { get; private set; }
        public string Provider { get; private set; }
        public DateRange Attended { get; private set; }
        public string? Description { get; private set; }

        [JsonConstructor]
        public CertificationUpdated(Guid aggregateId, Guid certificationId, string name, string provider, DateRange attended, string? description) 
            : base(aggregateId)
        {
            CertificationId = certificationId;
            Name = name;
            Provider = provider;
            Attended = attended;
            Description = description;
        }

    }
}
