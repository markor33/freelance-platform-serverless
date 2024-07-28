using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.SeedWork;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class ContractCreated : DomainEvent
    {
        public Contract Contract { get; private set; }

        public ContractCreated(Guid aggregateId, Contract contract) : base(aggregateId)
        {
            Contract = contract;
        }
    }
}
