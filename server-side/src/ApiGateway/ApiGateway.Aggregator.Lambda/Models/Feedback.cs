using FeedbackManagement.Persistence;

namespace ApiGateway.Aggregator.Lambda.Models;

public class Feedback
{
    public Guid JobId { get; set; }
    public string JobTitle { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; }

    public Feedback(FinishedContract finishedContract, string jobTitle)
    {
        JobId = finishedContract.JobId;
        JobTitle = jobTitle;
        Rating = finishedContract.ClientFeedback.Rating;
        Text = finishedContract.ClientFeedback.Text;
    }
}
