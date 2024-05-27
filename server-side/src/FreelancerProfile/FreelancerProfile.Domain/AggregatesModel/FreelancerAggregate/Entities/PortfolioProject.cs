using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.ValueObjects;
using FreelancerProfile.Domain.SeedWork;

namespace FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities
{
    public class PortfolioProject : Entity<Guid>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateRange Period { get; private set; }
        // public List<string> Files { get; private set; }

        public PortfolioProject() { }

        public PortfolioProject(string title, string description, DateRange period)
        {
            Title = title;
            Description = description;
            Period = period;
        }

    }
}
