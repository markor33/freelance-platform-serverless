using ClientProfile.Lambda;
using ReadModel;

namespace ApiGateway.Aggregator.Lambda.Models;

public class Chat
{
    public Guid Id { get; set; }
    public Guid JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public Guid ProposalId { get; set; }
    public Guid? ContractId { get; set; } = null;
    public Guid ClientId { get; set; }
    public bool IsClientActive { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public Guid FreelancerId { get; set; }
    public bool IsFreelancerActive { get; set; }
    public string FreelancerName { get; set; } = string.Empty;

    public Chat(RealTime.Chat.Models.Chat chat, FreelancerViewModel freelancer, Client client, JobViewModel job)
    {
        Id = chat.Id;
        JobId = chat.Id;
        JobTitle = job.Title;
        ProposalId = chat.ProposalId;
        ClientId = chat.Id;
        IsClientActive = true;
        ClientName = client.FirstName + " " + client.LastName;
        FreelancerId = chat.Id;
        IsFreelancerActive = true;
        FreelancerName = freelancer.FirstName + " " + freelancer.LastName;

        if (chat.ContractId != null)
            chat.ContractId = chat.ContractId;
    }
}
