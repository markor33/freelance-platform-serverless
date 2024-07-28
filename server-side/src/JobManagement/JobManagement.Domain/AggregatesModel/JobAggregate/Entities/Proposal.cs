using JobManagement.Domain.SeedWork;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Entities
{
    public class Proposal : Entity<Guid>
    {
        public Guid FreelancerId { get; private set; }
        public string Text { get; private set; }
        public Payment Payment { get; private set; }
        public ProposalStatus? Status { get; private set; }
        public List<Answer> Answers { get; private set; } = new();
        public DateTime Created { get; private set; }

        public Proposal() { }

        [JsonConstructor]
        public Proposal(Guid id, Guid freelancerId, string text, Payment payment, ProposalStatus? status, List<Answer> answers, DateTime created)
        {
            Id = id;
            FreelancerId = freelancerId;
            Text = text;
            Payment = payment;
            Status = status;
            Answers = answers;
            Created = created;
        }

        public static Proposal Create(Guid freelancerId, string text, Payment payment, List<Answer> answers, ProposalStatus? status = null)
        {
            var proposal = new Proposal()
            {
                Id = Guid.NewGuid(),
                FreelancerId = freelancerId,
                Text = text,
                Payment = payment,
                Status = status,
                Answers = answers,
                Created = DateTime.UtcNow
            };

            return proposal;
        }

        public void AddAnswer(Answer answer)
        {
            Answers.Add(answer);
        }

        public void AddAnswers(List<Answer> answers)
        {
            Answers.AddRange(answers);
        }

        public void ChangeStatus(ProposalStatus status)
        {
            Status = status;
        }

        public void ChangePayment(Payment payment) => Payment = payment;

    }
}
