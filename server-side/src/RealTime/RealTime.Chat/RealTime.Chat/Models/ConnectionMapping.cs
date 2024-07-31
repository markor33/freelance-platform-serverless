using Amazon.DynamoDBv2.DataModel;

namespace RealTime.Chat.Models;

[DynamoDBTable("ChatConnectionMapping")]
public class ConnectionMapping
{
    [DynamoDBHashKey]
    public Guid Sub { get; set; }
    [DynamoDBGlobalSecondaryIndexHashKey]
    public string ConnectionId { get; set; }

}
