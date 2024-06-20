using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class ProfilePictureChanged : DomainEvent
    {
        public string ProfilePictureUrl { get; private set; }

        [JsonConstructor]
        public ProfilePictureChanged(Guid aggregateId, string profilePictureUrl) : base(aggregateId)
        {
            ProfilePictureUrl = profilePictureUrl;
        }

    }
}
