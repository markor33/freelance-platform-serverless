using FreelancerProfile.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects
{
    public class ProfileSummary : ValueObject
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        public ProfileSummary() { }

        [JsonConstructor]
        public ProfileSummary(string title, string description)
        {
            Title = title;
            Description = description;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Description;
        }
    }
}
