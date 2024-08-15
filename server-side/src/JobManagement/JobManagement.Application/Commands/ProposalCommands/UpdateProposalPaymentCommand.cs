using FluentResults;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using MediatR;
using System.Text.Json.Serialization;

namespace JobManagement.Application.Commands.ProposalCommands
{
    public class UpdateProposalPaymentCommand : IRequest<Result>
    {
        public Guid JobId { get; private set; }
        public Guid ProposalId { get; private set; }
        public Payment Payment { get; private set; }

        [JsonConstructor]
        public UpdateProposalPaymentCommand(Guid jobId, Guid proposalId, Payment payment)
        {
            JobId = jobId;
            ProposalId = proposalId;
            Payment = payment;
        }

    }
}
