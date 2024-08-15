using System.Text.Json.Serialization;

namespace ClientProfile.Lambda.IntegrationEvents;

public record ClientRegisteredIntegrationEvent
{
    public Guid UserId { get; init; }

    public ClientRegisteredIntegrationEvent() { }

    [JsonConstructor]
    public ClientRegisteredIntegrationEvent(Guid userId)
    {
        UserId = userId;
    }
}
