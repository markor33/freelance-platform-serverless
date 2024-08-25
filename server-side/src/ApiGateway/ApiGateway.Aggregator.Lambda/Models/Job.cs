using JobSearch.IndexModel;

namespace ApiGateway.Aggregator.Lambda.Models;

public class Job
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public Payment Payment { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }
    public int Credits { get; set; }
    public int NumOfProposals { get; set; }
    public int NumOfCurrInterviews { get; set; }
    public double ClientAverageRating { get; set; }

    public Job(JobSearch.IndexModel.Job indexJob, double clientAverageRating)
    {
        Id = indexJob.Id;
        ClientId = indexJob.ClientId;
        Title = indexJob.Title;
        Description = indexJob.Description;
        Created = indexJob.Created;
        Payment = indexJob.Payment;
        ExperienceLevel = indexJob.ExperienceLevel;
        Credits = indexJob.Credits;
        NumOfProposals = indexJob.NumOfProposals;
        NumOfCurrInterviews = indexJob.NumOfCurrInterviews;
        ClientAverageRating = clientAverageRating;
    }
}
