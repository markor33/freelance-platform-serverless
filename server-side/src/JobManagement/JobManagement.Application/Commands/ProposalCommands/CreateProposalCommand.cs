using FluentResults;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using MediatR;
using System.Text.Json.Serialization;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class CreateProposalCommand : IRequest<Result<Proposal>>
    {
        public Guid FreelancerId { get; private set; }
        public Guid JobId { get; private set; }
        public string Text { get; private set; }
        public Payment Payment { get; private set; }
        public List<Answer> Answers { get; private set; }

        [JsonConstructor]
        public CreateProposalCommand(Guid freelancerId, Guid jobId, string text, Payment payment, List<Answer> answers)
        {
            FreelancerId = freelancerId;
            JobId = jobId;
            Text = text;
            Payment = payment;
            Answers = answers;
        }

    }
}
