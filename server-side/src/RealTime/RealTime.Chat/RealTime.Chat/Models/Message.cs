using Amazon.DynamoDBv2.DataModel;

namespace RealTime.Chat.Models;

[DynamoDBTable("Messages")]
public class Message
{
    [DynamoDBHashKey]
    public Guid ChatId { get; set; }
    [DynamoDBRangeKey]
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    public Message() { }

    public Message(Guid chatId, Guid senderId, string text)
    {
        Id = Guid.NewGuid();
        ChatId = chatId;
        SenderId = senderId;
        Text = text;
        Date = DateTime.UtcNow;
    }
}
