using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Events
{
    public class JobUpdated : DomainEvent
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Credits { get; private set; }
        public ExperienceLevel ExperienceLevel { get; private set; }
        public Payment Payment { get; private set; }
        public List<Question> Questions { get; private set; }
        public Guid ProfessionId { get; private set; }
        public List<Skill> Skills { get; private set; }

        [JsonConstructor]
        public JobUpdated(Guid aggregateId, string title, string description, int credits,
            ExperienceLevel experienceLevel, Payment payment, List<Question> questions, Guid professionId, List<Skill> skills)
            : base(aggregateId)
        {
            Title = title;
            Description = description;
            Credits = credits;
            ExperienceLevel = experienceLevel;
            Payment = payment;
            Questions = questions;
            ProfessionId = professionId;
            Skills = skills;
        }

    }
}
