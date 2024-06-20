using FreelancerProfile.Domain.SeedWork;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities
{
    public class Profession : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Skill> Skills { get; private set; }

        public Profession()
        {
            Skills = new List<Skill>();
        }

        public Profession(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Profession(Guid id, string name, string description, List<Skill> skills)
        {
            Id = id;
            Name = name;
            Description = description;
            Skills = skills;
        }
    }
}
