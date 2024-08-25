using JobSearch.IndexModel;
using System.Text.Json.Serialization;

namespace JobSearch.Lambda.IntegrationEvents;

public class JobCreated
{
    public Guid AggregateId { get; private set; }
    public Guid ClientId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int Credits { get; private set; }
    public DateTime Created { get; private set; }
    public ExperienceLevel ExperienceLevel { get; private set; }
    public Payment Payment { get; private set; }
    public JobStatus Status { get; private set; }
    public Guid ProfessionId { get; private set; }

    [JsonConstructor]
    public JobCreated(Guid aggregateId, Guid clientId, string title, string description, DateTime created, int credits,
        ExperienceLevel experienceLevel, Payment payment, JobStatus status, Guid professionId)
    {
        AggregateId = aggregateId;
        ClientId = clientId;
        Title = title;
        Description = description;
        Created = created;
        Credits = credits;
        ExperienceLevel = experienceLevel;
        Payment = payment;
        Status = status;
        ProfessionId = professionId;
    }

}
