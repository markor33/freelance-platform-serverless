using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class ProfileSummaryUpdated : DomainEvent
    {
        public ProfileSummary ProfileSummary { get; private set; }

        [JsonConstructor]
        public ProfileSummaryUpdated(Guid aggregateId, ProfileSummary profileSummary) : base(aggregateId)
        {
            ProfileSummary = profileSummary;
        }

    }
}
