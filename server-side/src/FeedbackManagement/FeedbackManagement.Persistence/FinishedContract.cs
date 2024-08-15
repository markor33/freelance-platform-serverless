using Amazon.DynamoDBv2.DataModel;
using System.Text.Json.Serialization;

namespace FeedbackManagement.Persistence;

[DynamoDBTable("Feedback")]
public class FinishedContract
{
    [DynamoDBHashKey]
    public Guid Id { get; private set; }
    public Guid JobId { get; private set; }
    public Guid ClientId { get; private set; }
    public Feedback? ClientFeedback { get; private set; }
    public Guid FreelancerId { get; private set; }
    public Feedback? FreelancerFeedback { get; private set; }

    public FinishedContract() { }

    public FinishedContract(Guid id, Guid jobId, Guid clientId, Guid freelancerId)
    {
        Id = id;
        JobId = jobId;
        ClientId = clientId;
        FreelancerId = freelancerId;
    }

    public void SetClientFeedback(Feedback clientFeedback)
    {
        ClientFeedback = clientFeedback;
    }

    public void SetFreelancerFeedback(Feedback freelancerFeedback)
    {
        FreelancerFeedback = freelancerFeedback;
    }
}

public class Feedback
{
    public int Rating { get; private set; }
    public string Text { get; private set; }

    public Feedback() { }

    [JsonConstructor]
    public Feedback(int rating, string text)
    {
        Rating = rating;
        Text = text;
    }
}