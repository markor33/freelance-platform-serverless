using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities
{
    public class Skill : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        [JsonIgnore]
        public Profession Profession { get; private set; }
        [JsonIgnore]
        public List<Freelancer> Freelancers { get; private set; }

        public Skill() { }

        [JsonConstructor]
        public Skill(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

    }
}
