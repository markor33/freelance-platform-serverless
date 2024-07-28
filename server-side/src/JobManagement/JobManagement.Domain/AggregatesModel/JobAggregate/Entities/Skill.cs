using JobManagement.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace JobManagement.Domain.AggregatesModel.JobAggregate.Entities
{
    public class Skill : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        [JsonIgnore]
        public Profession Profession { get; private set; }
        [JsonIgnore]
        public List<Job> Freelancers { get; private set; }

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
