using Amazon.DynamoDBv2.DataModel;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;

namespace ReadModel;

[DynamoDBTable("ProposalReadModelStore")]
public record ProposalViewModel
{
    public Guid Id { get; private init; }
    public Guid JobId { get; private init; }
    public Guid FreelancerId { get; private init; }
    public string Text { get; private init; }
    public Payment Payment { get; set; }
    public ProposalStatus Status { get; set; }
    public DateTime Created { get; private init; }
    public List<AnswerViewModel> Answers { get; set; }

    public ProposalViewModel()
    {
        Answers= new List<AnswerViewModel>();
    }

    public ProposalViewModel(Guid id, Guid freelancerId, string text, Payment payment, ProposalStatus status, DateTime created, List<AnswerViewModel> answers)
    {
        Id = id;
        FreelancerId = freelancerId;
        Text = text;
        Payment = payment;
        Status = status;
        Created = created;
        Answers = answers;
    }

    public ProposalViewModel(Guid jobId, Proposal proposal, Dictionary<Guid, QuestionViewModel> questions)
    {
        Id = proposal.Id;
        JobId = jobId;
        FreelancerId = proposal.FreelancerId;
        Text = proposal.Text;
        Payment = proposal.Payment;
        if (proposal.Status != null) 
        {
            Status = (ProposalStatus)proposal.Status;
        }
        Created = proposal.Created;
        Answers = proposal.Answers.Select(x => new AnswerViewModel(x, questions.GetValueOrDefault(x.QuestionId))).ToList();
    }
}
