﻿using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class ContractStatusChanged : DomainEvent
    {
        public Guid ContractId { get; private set; }
        public ContractStatus Status { get; private set; }

        [JsonConstructor]
        public ContractStatusChanged(Guid aggregateId, Guid contractId, ContractStatus status) : base(aggregateId)
        {
            ContractId = contractId;
            Status = status;
        }
    }
}
