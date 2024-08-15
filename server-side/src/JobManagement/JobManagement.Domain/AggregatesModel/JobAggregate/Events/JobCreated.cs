using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class JobCreated : DomainEvent
    {
        public Guid ClientId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Credits { get; private set; }
        public DateTime Created { get; private set; }
        public ExperienceLevel ExperienceLevel { get; private set; }
        public Payment Payment { get; private set; }
        public JobStatus Status { get; private set; }
        public List<Question> Questions { get; private set; }
        public Guid ProfessionId { get; private set; }
        public List<Skill> Skills { get; private set; }

        [JsonConstructor]
        public JobCreated(Guid aggregateId, Guid clientId, string title, string description, DateTime created, int credits, 
            ExperienceLevel experienceLevel, Payment payment, JobStatus status, List<Question> questions, Guid professionId, List<Skill> skills)
            : base(aggregateId)
        {
            ClientId = clientId;
            Title = title;
            Description = description;
            Created = created;
            Credits = credits;
            ExperienceLevel = experienceLevel;
            Payment = payment;
            Status = status;
            Questions = questions;
            ProfessionId = professionId;
            Skills = skills;
        }

    }
}
