using Amazon.DynamoDBv2.DataModel;

namespace RealTime.Chat.Models;

[DynamoDBTable("Chats")]
public class Chat
{
    [DynamoDBHashKey]
    public Guid Id { get; set; }
    [DynamoDBGlobalSecondaryIndexHashKey("ClientIdIndex")]
    public Guid ClientId { get; set; }
    [DynamoDBGlobalSecondaryIndexHashKey("FreelancerIdIndex")]
    public Guid FreelancerId { get; set; }
    public Guid JobId { get; set; }
    public Guid ProposalId { get; set; }
    public Guid? ContractId { get; set; }

    public Chat() { }

    public Chat(Guid clientId, Guid freelancerId, Guid jobId, Guid proposalId)
    {
        Id = Guid.NewGuid();
        ClientId = clientId;
        FreelancerId = freelancerId;
        JobId = jobId;
        ProposalId = proposalId;
    }

    public void SetContractId(Guid contractId) => ContractId = contractId;
}
