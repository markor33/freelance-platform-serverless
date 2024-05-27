using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities
{
    public class Language : Entity<int>
    {
        public string Name { get; private set; }
        public string ShortName { get; private set; }

        public Language() { }

        public Language(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }

        [JsonConstructor]
        public Language(int id, string name, string shortName)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
        }

    }
}
