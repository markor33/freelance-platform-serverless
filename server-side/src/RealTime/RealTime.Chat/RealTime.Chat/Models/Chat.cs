using Amazon.DynamoDBv2.DataModel;

namespace RealTime.Chat.Models;

[DynamoDBTable("Chats")]
public class Chat
{
    [DynamoDBHashKey]
    public Guid Id { get; private set; }
    [DynamoDBGlobalSecondaryIndexHashKey("ClientIdIndex")]
    public Guid ClientId { get; private set; }
    [DynamoDBGlobalSecondaryIndexHashKey("FreelancerIdIndex")]
    public Guid FreelancerId { get; private set; }
    public Guid JobId { get; private set; }
    public Guid ProposalId { get; private set; }
    public Guid? ContractId { get; private set; }

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
