using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;

namespace ReserveCredits.Lambda;

public enum Result
{
    Success,
    Fail
}

public class CreditsSubstractionResult
{
    public Guid JobId { get; set; }
    public Proposal Proposal { get; set; }
    public Result Result { get; set; }

    public CreditsSubstractionResult(Guid jobId, Proposal proposal, Result result)
    {
        JobId = jobId;
        Proposal = proposal;
        Result = result;
    }
}
