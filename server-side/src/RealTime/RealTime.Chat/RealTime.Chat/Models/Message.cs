using Amazon.DynamoDBv2.DataModel;

namespace RealTime.Chat.Models;

[DynamoDBTable("Messages")]
public class Message
{
    [DynamoDBHashKey]
    public Guid ChatId { get; private set; }
    [DynamoDBRangeKey]
    public Guid Id { get; private set; }
    public Guid SenderId { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }

    public Message(Guid chatId, Guid senderId, string text)
    {
        Id = Guid.NewGuid();
        ChatId = chatId;
        SenderId = senderId;
        Text = text;
        Date = DateTime.UtcNow;
    }
}
