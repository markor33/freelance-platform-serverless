using Amazon.DynamoDBv2.DataModel;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;

namespace ReadModel;

[DynamoDBTable("ContractReadModelStore")]
public record ContractViewModel
{
    [DynamoDBHashKey]
    public Guid Id { get; set; }
    [DynamoDBRangeKey]
    public Guid JobId { get; set; }
    public Guid ClientId { get; set; }
    public Guid FreelancerId { get; set; }
    public Payment Payment { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Finished { get; set; }
    public ContractStatus Status { get; set; }

    public ContractViewModel() { }

    public ContractViewModel(Guid id, Guid jobId, Guid clientId, 
        Guid freelancerId, Payment payment, DateTime started, DateTime? finished,  ContractStatus status)
    {
        Id = id;
        JobId = jobId;
        ClientId = clientId;
        FreelancerId = freelancerId;
        Payment = payment;
        Started = started;
        Finished = finished;
        Status = status;
    }

}
