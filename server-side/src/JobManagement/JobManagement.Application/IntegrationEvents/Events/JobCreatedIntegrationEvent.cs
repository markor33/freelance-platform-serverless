using JobManagement.Domain.AggregatesModel.JobAggregate;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using System.Text.Json.Serialization;

namespace JobManagement.Application.IntegrationEvents.Events
{
    public record JobCreatedIntegrationEvent 
    {
        public Guid JobId { get; private set; }
        public Guid ClientId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Created { get; private set; }
        public int Credits { get; private set; }
        public ExperienceLevel ExperienceLevel { get; private set; }
        public Payment Payment { get; private set; }
        public JobStatus Status { get; private set; }
        public Guid ProfessionId { get; private set; }
        public List<Guid> Skills { get; private set; }

        public JobCreatedIntegrationEvent() { }

        public JobCreatedIntegrationEvent(Job job)
        {
            JobId = job.Id;
            ClientId = job.ClientId;
            Title = job.Title;
            Description = job.Description;
            Created = job.Created;
            Credits = job.Credits;
            ExperienceLevel = job.ExperienceLevel;
            Payment = job.Payment;
            Status = job.Status;
            ProfessionId = job.ProfessionId;
            Skills = job.Skills.Select(x => x.Id).ToList();
        }

        [JsonConstructor]
        public JobCreatedIntegrationEvent(Guid jobId, Guid clientId, string title, string description, DateTime created, 
            int credits, ExperienceLevel experienceLevel, Payment payment, JobStatus status, Guid professionId, List<Guid> skills)
        {
            JobId = jobId;
            ClientId = clientId;
            Title = title;
            Description = description;
            Created = created;
            Credits = credits;
            ExperienceLevel = experienceLevel;
            Payment = payment;
            Status = status;
            ProfessionId = professionId;
            Skills = skills;
        }
    }
}
