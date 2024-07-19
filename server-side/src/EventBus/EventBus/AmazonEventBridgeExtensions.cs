using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using System.Text.Json;

namespace EventBus;

public static class AmazonEventBridgeExtensions
{
    private static readonly string _eventBusName = Environment.GetEnvironmentVariable("EVENT_BUS");
    private static readonly string _serviceName = Environment.GetEnvironmentVariable("SERVICE_NAME");

    public static async Task<PutEventsResponse> PublishEvent<T>(this IAmazonEventBridge eventBridgeClient, T @event)
    {
        var request = new PutEventsRequest
        {
            Entries = new List<PutEventsRequestEntry>
            {
                new PutEventsRequestEntry
                {
                    EventBusName = _eventBusName,
                    Source = _serviceName,
                    DetailType = typeof(T).Name,
                    Detail = JsonSerializer.Serialize<T>(@event),
                }
            }
        };

        return await eventBridgeClient.PutEventsAsync(request);
    }

    public static async Task<PutEventsResponse> PublishEvent(this IAmazonEventBridge eventBridgeClient, string detailType, string detail)
    {
        var request = new PutEventsRequest
        {
            Entries = new List<PutEventsRequestEntry>
            {
                new PutEventsRequestEntry
                {
                    EventBusName = _eventBusName,
                    Source = _serviceName,
                    DetailType = detailType,
                    Detail = detail,
                }
            }
        };

        return await eventBridgeClient.PutEventsAsync(request);
    }
}

