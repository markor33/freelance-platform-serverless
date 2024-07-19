using System.Text.Json.Serialization;

namespace FreelancerCommands.Lambda.IntegrationEvents
{
    public record FreelancerRegisteredIntegrationEvent
    {
        public Guid UserId { get; init; }

        public FreelancerRegisteredIntegrationEvent() { }

        [JsonConstructor]
        public FreelancerRegisteredIntegrationEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
