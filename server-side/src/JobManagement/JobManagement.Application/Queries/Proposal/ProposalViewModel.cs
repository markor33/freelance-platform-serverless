using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;

namespace JobManagement.Application.Queries
{
    public record ProposalViewModel
    {
        public Guid Id { get; private init; }
        public Guid FreelancerId { get; private init; }
        public string Text { get; private init; }
        public Payment Payment { get; set; }
        public ProposalStatus? Status { get; private init; }
        public DateTime Created { get; private init; }
        public List<AnswerViewModel> Answers { get; set; }

        public ProposalViewModel()
        {
            Answers= new List<AnswerViewModel>();
        }

        public ProposalViewModel(Guid id, Guid freelancerId, string text, Payment payment, ProposalStatus? status, DateTime created, List<AnswerViewModel> answers)
        {
            Id = id;
            FreelancerId = freelancerId;
            Text = text;
            Payment = payment;
            Status = status;
            Created = created;
            Answers = answers;
        }
    }
}
