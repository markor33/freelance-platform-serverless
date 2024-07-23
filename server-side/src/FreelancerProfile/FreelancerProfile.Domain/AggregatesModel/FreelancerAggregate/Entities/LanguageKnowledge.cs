using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Enums;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities
{
    public class LanguageKnowledge : Entity<Guid>
    {
        public Language Language { get; private set; }
        public LanguageProficiencyLevel ProfiencyLevel { get; private set; }

        public LanguageKnowledge() { }

        [JsonConstructor]
        public LanguageKnowledge(Language language, LanguageProficiencyLevel profiencyLevel)
        {
            Id = Guid.NewGuid();
            Language = language;
            ProfiencyLevel = profiencyLevel;
        }

    }
}
