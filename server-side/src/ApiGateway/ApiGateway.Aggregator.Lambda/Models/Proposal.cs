using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using ReadModel;

namespace ApiGateway.Aggregator.Lambda.Models;

public class Proposal
{
    public Guid Id { get; private set; }
    public string Text { get; private set; }
    public Payment Payment { get; private set; }
    public ProposalStatus Status { get; private set; }
    public FreelancerBasic Freelancer { get; private set; }
    // public float FreelancerAverageRating { get; private set; }
    public DateTime Created { get; private set; }

    public Proposal(ProposalViewModel proposal, FreelancerViewModel freelancer)
    {
        Id = proposal.Id;
        Text = proposal.Text;
        Payment = proposal.Payment;
        Status = proposal.Status;
        Created = proposal.Created;
        Freelancer = new FreelancerBasic(freelancer);
    }
}
