using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class CertificationDeleted : DomainEvent
    {
        public Guid CertificationId { get; private set; }

        [JsonConstructor]
        public CertificationDeleted(Guid aggregateId, Guid certificationId) : base(aggregateId)
        {
            CertificationId = certificationId;
        }
    }
}
