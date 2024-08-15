using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using ReadModel;

namespace ApiGateway.Aggregator.Lambda.Models;

public class Contract
{
    public Guid Id { get; private init; }
    public Guid JobId { get; private init; }
    public string JobTitle { get; private init; }
    public Guid ClientId { get; private init; }
    public Guid FreelancerId { get; private init; }
    public string FreelancerName { get; private init; }
    public Payment Payment { get; set; }
    public DateTime Started { get; private init; }
    public DateTime? Finished { get; private init; }
    public ContractStatus Status { get; private init; }

    public Contract(JobViewModel job, ContractViewModel contract, FreelancerViewModel freelancer)
    {
        Id = contract.Id;
        JobId = contract.JobId;
        JobTitle = job.Title;
        ClientId = contract.ClientId;
        FreelancerId = contract.FreelancerId;
        FreelancerName = freelancer.FullName;
        Payment = contract.Payment;
        Started = contract.Started;
        Finished = contract.Finished;
        Status = contract.Status;
    }

    public Contract(ContractViewModel contract, FreelancerViewModel freelancer)
    {
        Id = contract.Id;
        JobId = contract.JobId;
        ClientId = contract.ClientId;
        FreelancerId = contract.FreelancerId;
        FreelancerName = freelancer.FullName;
        Payment = contract.Payment;
        Started = contract.Started;
        Finished = contract.Finished;
        Status = contract.Status;
    }
}
