using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Events
{
    public class SkillsUpdated : DomainEvent
    {
        public List<Skill> Skills { get; private set; }

        [JsonConstructor]
        public SkillsUpdated(Guid aggregateId, List<Skill> skills) : base(aggregateId)
        {
            Skills = skills;
        }
    }
}
