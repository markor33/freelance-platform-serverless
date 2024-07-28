using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;

namespace ReadModel
{
    public record ContractViewModel
    {
        public Guid Id { get; private init; }
        public Guid JobId { get; private init; }
        public string JobTitle { get; private init; }
        public Guid ClientId { get; private init; }
        public Guid FreelancerId { get; private init; }
        public Payment Payment { get; set; }
        public DateTime Started { get; private init; }
        public DateTime? Finished { get; private init; }
        public ContractStatus Status { get; private init; }

        public ContractViewModel() { }

        public ContractViewModel(Guid id, Guid jobId, string jobTitle, Guid clientId, 
            Guid freelancerId, Payment payment, DateTime started, DateTime? finished,  ContractStatus status)
        {
            Id = id;
            JobId = jobId;
            JobTitle = jobTitle;
            ClientId = clientId;
            FreelancerId = freelancerId;
            Payment = payment;
            Started = started;
            Finished = finished;
            Status = status;
        }

    }
}
