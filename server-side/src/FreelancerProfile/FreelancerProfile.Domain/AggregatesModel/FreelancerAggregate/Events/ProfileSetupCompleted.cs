﻿using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Enums;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class ProfileSetupCompleted : DomainEvent
    {
        public bool IsProfilePublic { get; private set; }
        public ProfileSummary ProfileSummary { get; private set; }
        public HourlyRate HourlyRate { get; private set; }
        public Availability Availability { get; private set; }
        public ExperienceLevel ExperienceLevel { get; private set; }
        public Guid ProfessionId { get; private set; }
        [JsonIgnore]
        public Profession Profession { get; private set; }
        public LanguageKnowledge LanguageKnowledge { get; private set; }

       public ProfileSetupCompleted(
            Guid freelancerId,
            bool isProfilePublic,
            ProfileSummary profileSummary,
            HourlyRate hourlyRate,
            Availability availability,
            ExperienceLevel experienceLevel,
            Profession profession,
            LanguageKnowledge languageKnowledge) : base(freelancerId)
        {
            IsProfilePublic = isProfilePublic;
            ProfileSummary = profileSummary;
            HourlyRate = hourlyRate;
            Availability = availability;
            ExperienceLevel = experienceLevel;
            ProfessionId = profession.Id;
            Profession = profession;
            LanguageKnowledge = languageKnowledge;
        }

        [JsonConstructor]
        public ProfileSetupCompleted(
            Guid aggregateId,
            bool isProfilePublic, 
            ProfileSummary profileSummary, 
            HourlyRate hourlyRate, 
            Availability availability, 
            ExperienceLevel experienceLevel, 
            Guid professionId, 
            LanguageKnowledge languageKnowledge) : base(aggregateId)
        {
            IsProfilePublic = isProfilePublic;
            ProfileSummary = profileSummary;
            HourlyRate = hourlyRate;
            Availability = availability;
            ExperienceLevel = experienceLevel;
            ProfessionId = professionId;
            LanguageKnowledge = languageKnowledge;
        }
    }
}
