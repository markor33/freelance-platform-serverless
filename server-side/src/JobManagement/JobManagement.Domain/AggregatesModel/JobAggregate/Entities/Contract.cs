using JobManagement.Domain.SeedWork;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Entities
{
    public class Contract : Entity<Guid>
    {
        public Guid ClientId { get; private set; }
        public Guid FreelancerId { get; private set; }
        public Payment Payment { get; private set; }
        public DateTime Started { get; private set; }
        public DateTime? Finished { get; private set; }
        public ContractStatus Status { get; private set; }

        public Contract() { }

        public Contract(Guid clientId, Guid freelancerId, Payment payment)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;
            FreelancerId = freelancerId;
            Payment = payment;
            Started = DateTime.UtcNow;
            Finished = null;
            Status = ContractStatus.ACTIVE;
        }

        [JsonConstructor]
        public Contract(Guid id, Guid clientId, Guid freelancerId, Payment payment, DateTime started, DateTime? finished, ContractStatus status)
        {
            Id = id;
            ClientId = clientId;
            FreelancerId = freelancerId;
            Payment = payment;
            Started = started;
            Finished = finished;
            Status = status;
        }

        public void ChangeStatus(ContractStatus status)
        {
            if (status == ContractStatus.FINISHED)
                Finished = DateTime.UtcNow;
            Status = status;
        }

    }
}
