using Amazon.EventBridge;
using Amazon.Lambda.Core;
using EventBus;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CDCDispatcher.Lambda;

public class CDCEvent
{
    public List<JsonElement> Records { get; set; }
}

public class CDCEventRecord
{
    public string EventId { get; set; }
    public string EventName { get; set; }
    public string EventSource { get; set; }
    public DynamoDbStreamRecord DynamoDb { get; set; }
}

public class DynamoDbStreamRecord
{
    public Dictionary<string, Dictionary<string, string>> Keys { get; set; }
    public Dictionary<string, Dictionary<string, string>> NewImage { get; set; }
}

public class Function
{
    private readonly IAmazonEventBridge _eventBridgeClient = new AmazonEventBridgeClient();

    public async Task FunctionHandler(CDCEvent @event, ILambdaContext context)
    {
        foreach (var record in @event.Records)
        {
            if (record.GetProperty("eventName").GetString() != "INSERT")
                continue;

            var dynamodb = record.GetProperty("dynamodb");
            context.Logger.LogInformation(dynamodb.ToString());

            dynamodb.TryGetProperty("NewImage", out var newImage);

            newImage.TryGetProperty("EventType", out var eventType);
            newImage.TryGetProperty("EventData", out var eventData);


            var domainEventName = eventType.GetProperty("S").GetString();
            var domainEventData = eventData.GetProperty("S").GetString();

            await _eventBridgeClient.PublishEvent(domainEventName, domainEventData);

            context.Logger.LogInformation("Event published");
        }
    }
}
