namespace JobSearch.IndexModel;

public class Job
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid ProfessionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public JobStatus Status { get; set; }
    public Payment Payment { get; set; }
    public int Credits { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }
    public int NumOfProposals { get; set; }
    public int NumOfCurrInterviews { get; set; }
}

public class Payment
{
    public float Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public PaymentType Type { get; set; }
}

public enum PaymentType
{
    FIXED_RATE = 0,
    HOURLY_RATE
}

public enum ExperienceLevel
{
    JUNIOR = 0,
    MEDIOR,
    SENIOR
}

public enum JobStatus
{
    LISTED,
    IN_PROGRESS,
    DONE,
    REMOVED
}

public enum ProposalStatus
{
    SENT,
    INTERVIEW,
    CLIENT_APPROVED,
    FREELANCER_APPROVED
}