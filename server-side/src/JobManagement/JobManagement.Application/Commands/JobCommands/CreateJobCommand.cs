using MediatR;
using FluentResults;
using System.Text.Json.Serialization;
using JobManagement.Domain.AggregatesModel.JobAggregate;
using JobManagement.Domain.AggregatesModel.JobAggregate.Enums;
using JobManagement.Domain.AggregatesModel.JobAggregate.ValueObjects;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;

namespace JobManagement.Application.Commands.JobCommands
{
    public class CreateJobCommand : IRequest<Result<Job>>
    {
        public Guid ClientId { get; set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public ExperienceLevel ExperienceLevel { get; private set; }
        public Payment Payment { get; private set; }
        public List<Question> Questions { get; private set; }
        public Guid ProfessionId { get; private set; }
        public List<Guid> Skills { get; private set; }

        [JsonConstructor]
        public CreateJobCommand(Guid clientId, string title, string description,
            ExperienceLevel experienceLevel, Payment payment, List<Question> questions, Guid professionId, List<Guid> skills)
        {
            ClientId = clientId;
            Title = title;
            Description = description;
            ExperienceLevel = experienceLevel;
            Payment = payment;
            Questions = questions;
            ProfessionId = professionId;
            Skills = skills;
        }
    }
}
