using System.Text.Json;
using static Amazon.Lambda.SQSEvents.SQSEvent;

namespace Common.Layer.Extensions;

public static class SQSEventExtensions
{
    public static T DeserializeSNSMessage<T>(this SQSMessage sqsMessage)
    {
        var messageAttributes = JsonSerializer.Deserialize<Dictionary<string, string>>(sqsMessage.Body) ?? throw new Exception("SQS Message is most likely not SNS Notification");

        var obj = JsonSerializer.Deserialize<T>(messageAttributes["Message"]) ?? throw new Exception("SQS Message is most likely not SNS Notification");

        return obj;
    }
}
